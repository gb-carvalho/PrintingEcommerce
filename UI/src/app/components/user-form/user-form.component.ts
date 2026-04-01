import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthService, User } from '../../services/auth/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-form',
  standalone: false,
  templateUrl: './user-form.component.html',
  styleUrl: './user-form.component.css'
})
export class UserFormComponent {


  constructor(
    private route: ActivatedRoute,
    private authService: AuthService,
    private router: Router
  ) { }

  user: User = {
    id: '',
    name: '',
    email: '',
    role: '',   
    password: ''
  };

  error: string = "";
  isEdit: boolean = false;

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    this.isEdit = id ? true : false;

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

  persistUser(): void {
    console.log(this.isEdit);
    if (this.isEdit) {
      this.authService.updateUser(this.user).subscribe(response => {
        alert('Produto editado com sucesso!');
      });
    } else {
      this.authService.createUser(this.user).subscribe({
        next: (response) => {
          this.router.navigate(['users/list/']);
          alert('Produto criado com sucesso!');
        },
        error: (err) => {
          this.error = ""
          err.error.forEach((item: any, index: number) => {
            this.error += item.description + "\n";
          });
          console.log(err);
        }
      });
    }
  }
}
