import { Component } from '@angular/core';
import { Piece } from '../services/piece'; 
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-pieces',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './pieces.html',
  styleUrls: ['./pieces.css'],
})
export class Pieces {
  name = '';

  constructor(private pieceService: Piece) {}

  add() {
    if (!this.name) return;
    this.pieceService.addPiece(this.name).subscribe({
      next: () => {
        console.log('Pièce ajoutée !');
        this.name = '';
      },
      error: (err) => console.error(err)
    });
  }
}

