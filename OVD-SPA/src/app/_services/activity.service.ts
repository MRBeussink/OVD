import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ActivityService {
messages = [];

constructor() {
  this.messages.push('Now listening for activity.');
}

addMessage(msg: string) {
  this.messages.push(msg);
}

}
