import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatListModule } from '@angular/material/list';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    MatSidenavModule,
    MatToolbarModule,
    MatListModule,
    RouterModule
  ],
  template: `
    <mat-sidenav-container class="container">

      <!-- Sidebar -->
      <mat-sidenav mode="side" opened class="sidenav">
        <h2 class="logo">Korp ERP</h2>

        <mat-nav-list>
          <a mat-list-item routerLink="/produtos">Produtos</a>
          <a mat-list-item routerLink="/notas">Notas Fiscais</a>
        </mat-nav-list>
      </mat-sidenav>

      <!-- Conteúdo -->
      <mat-sidenav-content>
        <mat-toolbar color="primary">
          Sistema de NF-e
        </mat-toolbar>

        <div class="content">
          <router-outlet></router-outlet>
        </div>
      </mat-sidenav-content>

    </mat-sidenav-container>
  `,
  styles: [`
    .container {
      height: 100vh;
    }

    .sidenav {
      width: 220px;
      padding: 10px;
      background: #1e1e2f;
      color: white;
    }

    .logo {
      margin: 10px;
      font-size: 18px;
    }

    .content {
      padding: 20px;
    }
  `]
})
export class AppComponent {}