import { Component, OnInit } from '@angular/core';
import { ActivityService } from '../_services/activity.service';

@Component({
  selector: 'app-activity',
  templateUrl: './activity.component.html',
  styleUrls: ['./activity.component.css']
})
export class ActivityComponent implements OnInit {

  constructor(private activityService: ActivityService) { }

  ngOnInit() {
  }

  getMessages() {
    return this.activityService.messages;
  }

}
