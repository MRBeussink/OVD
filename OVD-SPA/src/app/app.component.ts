import { Component, OnInit } from '@angular/core';
import { AuthService } from './_services/auth.service';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
// We implement OnInit in case the user refreshes the page, we need to be able to pull the saved token
export class AppComponent implements OnInit {
  jwtHelper = new JwtHelperService();

  // Load the AuthService as a variable
  constructor(private authService: AuthService) {}

  // This runs any time the page loads
  ngOnInit() {
    // Grab the token from the user
    const token = localStorage.getItem('token');
    if (token) { // If the token exists
      // Decode it and pass it to authService
      this.authService.decodedToken = this.jwtHelper.decodeToken(token);
    }
  }
}
