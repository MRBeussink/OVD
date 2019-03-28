import { Component, OnInit } from '@angular/core';
import { GroupService } from '../_services/group.service';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-groups',
  templateUrl: './groups.component.html',
  styleUrls: ['./groups.component.css']
})
export class GroupsComponent implements OnInit {

  constructor(public groupService: GroupService, public authService: AuthService, public alertifyService: AlertifyService) { }

  ngOnInit() {}

  getGroupsAlphabetical() {
    return this.groupService.getGroups();
  }

  remove(group_name: string) {
    this.alertifyService.confirm('Are you sure you want to delete the ' + group_name + ' group?', () => {
      this.groupService.delete(group_name);
      this.getGroupsAlphabetical();
    });
  }

  edit(groupName: string) {
    this.groupService.activateGroup(groupName);
  }

}
