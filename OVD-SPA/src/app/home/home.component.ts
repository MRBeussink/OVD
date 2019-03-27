import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  model: any = {};

  constructor(private router: Router, public authService: AuthService) { }

  ngOnInit() {
  }

  login() {
    this.authService.login(this.model).subscribe(next => { // What to do when successful

    }, error => { // What to do on error

    }, () => { // What to do after success
      // Admins go to dashboard, users to user page
      if (this.isAdmin()) {
        this.router.navigate(['/dashboard']);
      } else {
        this.router.navigate(['']);
      }
    });

    // Clear login form
    this.model.username = '';
    this.model.password = '';
  }

  isAdmin() {
    return this.authService.isAdmin();
  }

}
