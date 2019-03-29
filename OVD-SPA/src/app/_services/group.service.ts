import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { ActivityService } from '../_services/activity.service';
import { environment } from '../../environments/environment';
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
      image: 'Commodore 64',
      hotspares: 0,
      total: 200,
      online: 0,
      active: 0,
      dawgtags: ['SIU853656388', 'SIU853656384', 'SIU853656366']
    },
    {
      name: 'CS410 Pen Test',
      image: 'Debster',
      hotspares: 0,
      total: 15,
      online: 0,
      active: 0,
      dawgtags: ['SIU853656388', 'SIU853656384', 'SIU853656366']
    },
    {
      name: 'CS434 Data Lab',
      image: 'Ubusoft',
      hotspares: 0,
      total: 4,
      online: 0,
      active: 0,
      dawgtags: ['SIU853656388', 'SIU853656384', 'SIU853656366']
    }
  ];

  constructor(private http: HttpClient, public authService: AuthService,
              private alertifyService: AlertifyService, private activityService: ActivityService) {
    // These are hard coded samples, remove these for the final product
    this.activityService.addMessage('Creating fake groups and adding to the group list.');
    this.groups.push(
      {
        name: 'Basic Lab',
        image: 'Macrosoft Winders',
        hotspares: 0,
        total: 40,
        online: 0,
        active: 0,
        dawgtags: ['SIU853656547', 'SIU853656723', 'SIU853654577']
      }
    );
    this.activityService.addMessage('Creating fake image list.');
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
      this.activityService.addMessage('Created the group "' + model.name + '".');
      return true;
  }
  this.alertifyService.error('Error creating group.');
  this.activityService.addMessage('Failed to create group "' + model.name + '".');
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
      this.activityService.addMessage('Deleted group "' + groupName + '".');
      return true;
    }
  }
  this.alertifyService.error('Error finding group.');
  this.activityService.addMessage('Could not delete group "' + groupName + '" because it could not be found.');
  return false;
}

edit(model: any) {
  // Basic input checks before the heavy lifting
  if (model.hotspares < 0 || model.hotspares % 1 !== 0) {
    this.alertifyService.error('Hotspares should be positive and whole.');
    return false;
  }
  if (model.total < 0 || model.total % 1 !== 0) {
    this.alertifyService.error('Total should be positive and whole.');
    return false;
  }

  // TODO: Note total being less than active/online

  // Search for the group in the list
  for (let i = 0; i < this.groups.length; i++) {
    if (model.name === this.groups[i].name) {
      const splitDawgtags = model.dawgtags.split('\n');
      this.groups[i].image = model.image;
      this.groups[i].hotspares = model.hotspares;
      this.groups[i].total = model.total;
      this.groups[i].dawgtags = splitDawgtags;
      this.alertifyService.success('Group edited.');
      this.activityService.addMessage('Edited the group "' + model.name + '".');
      return true;
    }
  }

  this.alertifyService.error('Error editing group, maybe the group no longer exists?');
  this.activityService.addMessage('Could not edit the group "' + model.name + '".');
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
