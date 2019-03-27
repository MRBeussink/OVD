import { Injectable } from '@angular/core';
declare let alertify: any;

@Injectable({
  providedIn: 'root'
})
export class AlertifyService {

constructor() { }

confirm(message: string, okCallback: () => any) {
  alertify.confirm(message, function(response) {
    console.log(response);
    if (response) {
      return okCallback();
    }
  });
}

newGroup() {
  alertify.prompt(
    'New Group',
    'Group Name', 'name',
    'Hot Spares', 'spares',
    'Total VMs', 'total',
    'Image', 'image',
    'Users', 'user'
  , function(evt, value) {
    console.log(name + spares + total + image + user);
  }
  , function() { });
}

}
