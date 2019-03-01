import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  // Grab input
  model: any = {};

  // Nothing to do on init
  ngOnInit() {}

  // If the username is name then fake a login
  login() {
    if (this.model.username === 'name') {
      console.log('Congrats');
    } else {
      console.log('nope');
    }
  }

  // Lets just hardcode where we are
  loggedIn() {
    return true;
  }

  // Lets hardcode whether we're admin
  isAdmin() {
    return false;
  }
}
