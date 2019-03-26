import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../_services/auth.service';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class GroupService {
  baseUrl = environment.apiURL + 'realadmin' + '/NewGroup/';
  groups = new Map();

  constructor(private http: HttpClient, public authService: AuthService) {
    // These are hard coded samples, remove these for the final product
    this.groups.set('Name', [3, 8, 2]);
    this.groups.set('Sample', [5, 7, 3]);
    this.groups.set('CS499 Test Group', [250, 300, 50]);
  }

create(model: any) {
  // Call the api to create a group returning true if it was successful
  // return this.http.post(this.baseUrl + 'create', model);
  if (this.groups.has(model.group_name)) {
    return false;
  }
  this.groups.set(model.group_name, [0, 10, 3]);
  return true;
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

}
