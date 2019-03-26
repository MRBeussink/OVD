import { Component, OnInit } from '@angular/core';
import { GroupService } from '../_services/group.service';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-groups',
  templateUrl: './groups.component.html',
  styleUrls: ['./groups.component.css']
})
export class GroupsComponent implements OnInit {

  constructor(public groupService: GroupService, public authService: AuthService) { }

  ngOnInit() {}

  getGroupsAlphabetical() {
    return this.groupService.groups;
  }

  remove(group_name: String) {
    this.groupService.delete(group_name);
  }

}
