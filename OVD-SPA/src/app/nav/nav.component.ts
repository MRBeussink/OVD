import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  // Grab input
  model: any = {};

  constructor(public authService: AuthService) { }

  // Nothing to do on init
  ngOnInit() {}

  // Pass the credentials to the api to attempt a login
  login() {
    this.authService.login(this.model).subscribe(next => {
      // What to do when successful
      console.log('success');
    }, error => {
      // What to do on error
      console.log('error');
    }, () => {
      // Where to take the user after
      console.log('logged in');
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

  logout() {
    this.authService.logout();
  }
}
