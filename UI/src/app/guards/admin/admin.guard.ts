import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core'
import { AuthService } from '../../services/auth/auth.service'
 
export const adminGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.hasRole('Admin')) return true;
  else {
    router.navigate(['/']);
    return false;
  }
};
