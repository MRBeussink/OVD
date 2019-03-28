import { Component, OnInit } from '@angular/core';
import { GroupService } from '../_services/group.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-new-group',
  templateUrl: './new-group.component.html',
  styleUrls: ['./new-group.component.css']
})
export class NewGroupComponent implements OnInit {
  model: any = {};

  constructor(public groupService: GroupService, private router: Router) { }

  ngOnInit() {
    // this.model = this.groupService.getGroup(this.groupService.activeGroup);
  }

  create() {
    if (this.groupService.create(this.model)) {
      this.clearForm();
      this.router.navigate(['/groups']);
    } else {
      console.log('error creating group');
    }
  }

  // Clears the entire form and backs the user out of the page
  clearForm() {
    console.log('clearing form');
    this.model.group_name = '';
    this.model.total_vm = '';
    this.model.hotspares = '';
    this.model.image = '';
    this.model.dawgtag = '';
  }

  getImages() {
     return this.groupService.getImages();
  }

}
