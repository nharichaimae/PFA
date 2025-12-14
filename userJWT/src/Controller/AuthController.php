<?php

namespace App\Controller;

use App\Entity\AuthToken;
use App\Repository\UserRepository;
use Doctrine\ORM\EntityManagerInterface;
use Symfony\Bundle\FrameworkBundle\Controller\AbstractController;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\HttpFoundation\JsonResponse;
use Symfony\Component\Routing\Annotation\Route;

class AuthController extends AbstractController
{
    #[Route('/api/login', name: 'api_login', methods: ['POST'])]
    public function login(
        Request $request,
        UserRepository $userRepository,
        EntityManagerInterface $em
    ): JsonResponse
    {
        $data = json_decode($request->getContent(), true);

        if (!$data || !isset($data['email'], $data['password'])) {
            return new JsonResponse(['message' => 'Email et mot de passe requis'], 400);
        }

        $user = $userRepository->findOneBy(['email' => $data['email']]);
        if (!$user) {
            return new JsonResponse(['authenticated' => false, 'message' => 'Utilisateur introuvable'], 401);
        }

        if ($user->getPassword() !== $data['password']) {
            return new JsonResponse(['authenticated' => false, 'message' => 'Mot de passe incorrect'], 401);
        }

        
        $tokenString = bin2hex(random_bytes(32));//+>la creation d'un token aleratoire pour l'utiliser .

        // +> creation de l'entite auth token 
        $authToken = new AuthToken();
        $authToken->setUser($user);
        $authToken->setToken($tokenString);
        $authToken->setExpirestAt(new \DateTime('+1 day')); // =>la date limite de token c'est a dire un joure 

        $em->persist($authToken);
        $em->flush();

        //quand va verifier les valeur avec les valeur de la base de donne si correcte va retourn un json avec le token et id et email 
        return new JsonResponse([
            'authenticated' => true,
            'token' => $tokenString,
            'user' => [
                'id' => $user->getId(),
                'email' => $user->getEmail()
            ]
        ]);
    }
    //recupere l'id et email pour que peut poser dans la table piece id cote .net 
    #[Route('/api/user-by-token', name: 'api_user_by_token', methods: ['GET'])]
        public function getUserByToken(Request $request, EntityManagerInterface $em): JsonResponse
        {
            $token = $request->query->get('token');
            if (!$token) {
                return new JsonResponse(['message' => 'Token manquant'], 400);
            }

            $authToken = $em->getRepository(AuthToken::class)->findOneBy(['token' => $token]);
            if (!$authToken || $authToken->getExpirestAt() < new \DateTime()) {
                return new JsonResponse(['message' => 'Token invalide ou expiré'], 401);
            }

            $user = $authToken->getUser();
            return new JsonResponse([
                'id' => $user->getId(),
                'email' => $user->getEmail()
            ]);
        }

}
