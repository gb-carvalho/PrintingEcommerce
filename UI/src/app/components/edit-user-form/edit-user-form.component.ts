import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthService, User } from '../../services/auth/auth.service';

@Component({
  selector: 'app-edit-user-form',
  standalone: false,
  templateUrl: './edit-user-form.component.html',
  styleUrl: './edit-user-form.component.css'
})
export class EditUserFormComponent {


  constructor(
    private route: ActivatedRoute,
    private authService: AuthService
  ) { }

  user: User = {
    id: '',
    name: '',
    email: '',
    role: ''
  };

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');

    if (id) {
      this.authService.getUser(id).subscribe({
        next: (user) => {
          //console.log(user.email);
          this.user.id = user.id;
          this.user.name = user.name;
          this.user.email = user.email;
          this.user.role = user.role;
        },
        error: (err) => {
          console.error('Erro ao buscar usuário', err);
        }
      });
    }
  }

  updateUser(): void {
    this.authService.updateUser(this.user).subscribe(response => {
      alert('Produto editado com sucesso!');
    });
  }
}
