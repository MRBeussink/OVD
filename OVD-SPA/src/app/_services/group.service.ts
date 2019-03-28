import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { environment } from '../../environments/environment';
import { GroupsComponent } from '../groups/groups.component';
import { Group } from '../_models/group';

@Injectable({
  providedIn: 'root'
})
export class GroupService {
  baseUrl = environment.apiURL + 'realadmin' + '/NewGroup/';
  images = [];
  activeGroup = 0;
  groups: Group[] = [
    {
      name: 'CS499 Test Group',
      image: 'Ubusoft',
      hotspares: 0,
      total: 0,
      online: 0,
      active: 0,
      dawgtags: ['SIU853656388', 'SIU853656384', 'SIU853656366']
    },
    {
      name: 'CS410 Pen Test',
      image: 'Ubusoft',
      hotspares: 0,
      total: 0,
      online: 0,
      active: 0,
      dawgtags: ['SIU853656388', 'SIU853656384', 'SIU853656366']
    },
    {
      name: 'CS434 Data Lab',
      image: 'Ubusoft',
      hotspares: 0,
      total: 0,
      online: 0,
      active: 0,
      dawgtags: ['SIU853656388', 'SIU853656384', 'SIU853656366']
    }
  ];

  constructor(private http: HttpClient, public authService: AuthService, private alertifyService: AlertifyService) {
    // These are hard coded samples, remove these for the final product
    this.groups.push(
      {
        name: 'Just a Pop',
        image: 'Ubusoft',
        hotspares: 0,
        total: 0,
        online: 0,
        active: 0,
        dawgtags: ['SIU853656547', 'SIU853656723', 'SIU853654577']
      }
    );
    this.images = ['Ubusoft', 'Macrosoft Winders', 'Orangintosh X', 'Debster', 'Commodore 64'];
  }

create(model: any) {
  console.log(model);
  if (this.groupsContains(model.name)) {
    this.alertifyService.error('Group already exists.');
    return false;
  }
  if (model.hotspares < 0 || model.hotspares % 1 !== 0) {
    this.alertifyService.error('Hotspares should be positive and whole.');
    return false;
  }
  if (model.total < 0 || model.total % 1 !== 0) {
    this.alertifyService.error('Total should be positive and whole.');
    return false;
  }
  const splitDawgtags = model.dawgtags.split('\n');
  this.groups.push(
    {
      name: model.name,
      image: model.image,
      hotspares: model.hotspares,
      total: model.total,
      online: 0,
      active: 0,
      dawgtags: splitDawgtags
    });
    // model.dawgtags.forEach(dawgtag => {
    //   this.groups[this.groups.length].dawgtags.push(dawgtag);
    // });
    if (this.groupsContains(model.name)) {
      this.alertifyService.success('Group created.');
      console.log(this.groups[this.groups.length - 1]);
      return true;
  }
  this.alertifyService.error('Error creating group.');
  return false;
}

groupsContains(groupName: string) {
  for (let i = 0; i < this.groups.length; i++) {
    if (groupName === this.groups[i].name) {
      return true;
    }
  }
  return false;
}

delete(groupName: string) {
  for (let i = 0; i < this.groups.length; i++) {
    if (groupName === this.groups[i].name) {
      this.groups.splice(i, 1);
      this.alertifyService.success('Group deleted.');
      return true;
    }
  }
  this.alertifyService.error('Error finding group.');
  return false;
}

edit(model: any, old: string) {
  if (model.name !== old && this.groupsContains(model.name)) {
    this.alertifyService.error('Group already exists.');
    return false;
  } else {
    for (let i = 0; i < this.groups.length; i++) {
      if (old === this.groups[i].name) {
        this.groups.splice(i, 1);
        break;
      }
    }
  }
  if (model.hotspares < 0 || model.hotspares % 1 !== 0) {
    this.alertifyService.error('Hotspares should be positive and whole.');
    return false;
  }
  if (model.total < 0 || model.total % 1 !== 0) {
    this.alertifyService.error('Total should be positive and whole.');
    return false;
  }
  const splitDawgtags = model.dawgtags.split('\n');
  if (this.groups.push(
    {
      name: model.name,
      image: model.image,
      hotspares: model.hotspares,
      total: model.total,
      online: 0,
      active: 0,
      dawgtags: splitDawgtags
    })) {
      this.alertifyService.success('Group edited.');
      return true;
    }
  this.alertifyService.error('Error editing group.');
  return false;
}

getGroups() {
  return this.groups;
}

getGroup(index: number) {
  return this.groups[index];
}

activateGroup(groupName: string) {
  for (let i = 0; i < this.groups.length; i++) {
    if (groupName === this.groups[i].name) {
      this.activeGroup = i;
      return true;
    }
  }
  return false;
}

getImages() {
  return this.images;
}

}
