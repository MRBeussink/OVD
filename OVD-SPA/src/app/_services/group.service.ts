import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../_services/auth.service';
import { environment } from '../../environments/environment';
import { GroupsComponent } from '../groups/groups.component';

@Injectable({
  providedIn: 'root'
})
export class GroupService {
  baseUrl = environment.apiURL + 'realadmin' + '/NewGroup/';
  activeGroup = '';
  editType = '';
  groups = new Map();
  images = [];

  constructor(private http: HttpClient, public authService: AuthService) {
    // These are hard coded samples, remove these for the final product
    this.groups.set('Name', [3, 8, 2]);
    this.groups.set('Sample', [5, 7, 3]);
    this.groups.set('CS499 Test Group', [250, 300, 50]);
    this.images = ['Ubuntu 1.6', 'Windows 10 U1.2', 'Debian 4.8', 'MS DOS v4'];
  }

create(model: any) {
  // Call the api to create a group returning true if it was successful
  // return this.http.post(this.baseUrl + 'create', model);
  if (this.groups.has(model.group_name)) {
    return false;
  }
  const running = Math.floor(Math.random() * model.total_vm);
  const active = Math.floor(Math.random() * running);

  this.groups.set(model.group_name, [active, running, model.total_vm - running]);
  return true;
}

update(oldName: String, model: any) {
  if (this.groups.has(oldName) && !this.groups.has(model.group_name)) {
    this.delete(oldName);
    this.create(model);
    return true;
  }
  return false;
}

delete(group: String) {
  // /delete/GroupName is the actual Url
  // return this.http.post(this.baseUrl + 'delete', model);
  return this.groups.delete(group);
}

getGroups() {
  console.log(this.http.get(this.baseUrl));
  return this.groups;
}

getGroup(name: String) {
  console.log(this.groups.get(name));
}

getImages() {
  return this.images;
}

}
