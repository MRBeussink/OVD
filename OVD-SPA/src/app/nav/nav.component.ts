import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { NavAuthService } from './navauth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};

 constructor(private authService: NavAuthService) { }

  ngOnInit() {
  }

  login() {
    // if (this.model.username === 'thatGuy') {
      this.authService.login(this.model).subscribe(next => {
        console.log('Congrats');
      }, error => {
        console.log('Failed to login');
      });
    // } else {
    //   console.log('nope');
    }
  // }

  loggedIn() {
    const token = 'true';
    return !!token;
  }

  logout() {
    console.log('logged out');
  }
}
