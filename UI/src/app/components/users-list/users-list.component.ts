import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';


@Component({
  selector: 'app-users-list',
  standalone: false,
  templateUrl: './users-list.component.html',
  styleUrl: './users-list.component.css'
})
export class UsersListComponent {

  users: any[] = [];

  constructor(public authService: AuthService, private router: Router) { }

  ngOnInit(): void {
    this.authService.getUsers().subscribe({
      next: (data) => this.users = data,
      error: (err) => console.error('Erro ao carregar usuários', err)
    });
  }

  editUser(id: string) {
    this.router.navigate(['users/edit/', id]);   
  }

  createUser() {
    this.router.navigate(['users/create/']);
  }

  deleteUser(id: string) {
    console.log('Excluir usuário:', id);
    this.authService.deleteUser(id).subscribe(response => {
      this.users = this.users.filter(user => user.id !== id);
      alert('Usuário deletado com sucesso!');
    });
  }
}
