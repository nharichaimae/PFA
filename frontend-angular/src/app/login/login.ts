import { Component } from '@angular/core';
import { AuthService } from '../services/auth';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css'],
})
export class LoginComponent {
  email = '';
  password = '';

  constructor(private auth: AuthService, private router: Router) {}

  login() {
    
    this.auth.login(this.email, this.password).subscribe(res => {
      this.auth.saveToken(res.token);
       const user = res.user;
        localStorage.setItem('user_id', user.id.toString());
      this.router.navigate(['/dashboard']);
    });
  }
}
