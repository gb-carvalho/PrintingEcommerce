import { Component } from '@angular/core';
import { AuthService } from '../../services/auth/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: false,  
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  username: string = '';
  password: string = '';
  errorMessage: string = '';

  constructor(private authService: AuthService, private router: Router) { };

  onLogin(): void {
    this.authService.login(this.username, this.password).subscribe({
      next: (response: any) => {
        const token = response.token;
        this.authService.saveToken(token);
        this.router.navigate(['/']).then(() => {
          window.location.reload(); 
        });
      },
      error: (error) => {
        this.errorMessage = 'Credenciais Inválidas';
      }
    });
  }

}
