import { Component } from '@angular/core';
import {teams} from '../app/teams';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'IM-Production-UI';  
  teams = teams;
}

