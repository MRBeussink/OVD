import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  // Grab input
  model: any = {};

  // Load in the auth service for logging in and router for redirects
  constructor(public authService: AuthService, private router: Router) { }

  // Nothing to do on init
  ngOnInit() {}

  // Pass the credentials to the api to attempt a login
  // The redirects if successful
  login() {
    this.authService.login(this.model).subscribe(next => { // What to do when successful

    }, error => { // What to do on error

    }, () => { // What to do after success
      // Admins go to dashboard, users to user page
      if (this.isAdmin()) {
        this.router.navigate(['/dashboard']);
      } else {
        this.router.navigate(['/user']);
      }
    });
  }

  // Check the api to see if the user is logged in
  loggedIn() {
    return this.authService.loggedIn();
  }

  // Since there are no normal users yet, just check if logged in
  isAdmin() {
    return this.loggedIn();
  }

  // Deletes user's token and redirects to home page
  logout() {
    this.authService.logout();
    this.router.navigate(['/home']);
  }
}
