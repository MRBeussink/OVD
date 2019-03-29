import { Component, OnInit } from '@angular/core';
import { GroupService } from '../_services/group.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-edit-group',
  templateUrl: './edit-group.component.html',
  styleUrls: ['./edit-group.component.css']
})
export class EditGroupComponent implements OnInit {
  activeGroup: any = {};

  constructor(public groupService: GroupService, private router: Router) { }

  ngOnInit() {
    const group = this.groupService.getGroup(this.groupService.activeGroup);
    this.mapGroupToForm(group);
  }

  mapGroupToForm(group: any) {
    // group = this.groupService.getGroup(this.groupService.activeGroup);
    let dawgtagsForForm = '';
    console.log(group.dawgtags);

    if (group.dawgtags.length !== 0) {
     group.dawgtags.forEach( dawgtag => {
       dawgtagsForForm += dawgtag + '\n';
     });
    }
     this.activeGroup.name = group.name;
     this.activeGroup.hotspares = group.hotspares;
     this.activeGroup.total = group.total;
     this.activeGroup.image = group.image;
    //  this.activeGroup.online = group.online;
    //  this.activeGroup.active = group.active;
     this.activeGroup.dawgtags = dawgtagsForForm;
  }

  edit() {
    if (this.groupService.edit(this.activeGroup)) {
      this.clearForm();
      this.router.navigate(['/groups']);
    }
  }

  getImages() {
     return this.groupService.getImages();
  }

  clearForm() {
    console.log('clearing form');
    this.activeGroup.group_name = '';
    this.activeGroup.total_vm = '';
    this.activeGroup.hotspares = '';
    this.activeGroup.image = '';
    this.activeGroup.dawgtag = '';
  }
}
