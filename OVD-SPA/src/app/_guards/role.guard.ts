import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';
import { AuthService } from '../_services/auth.service';

@Injectable({
    providedIn: 'root'
  })
export class RoleGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}
  canActivate(): boolean {
    if ( !this.authService.loggedIn() || !this.authService.isAdmin() ) {
        console.log('Failed to authenticate as admin');
      this.router.navigate(['login']);
      return false;
    }
    console.log('Admin detected');
    return true;
  }
}
