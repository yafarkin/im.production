import { Component } from '@angular/core';
import { OnInit } from '@angular/core';
import {Team} from './team';
import { TeamsService} from './teamsService';

@Component({
  selector: 'teams-grid',
  templateUrl: './teams-grid.html',
  styleUrls: ['./teams-grid.scss'],
  providers: [TeamsService]
})



export class TeamsGrid implements OnInit{

  teams: Team[]; 

  constructor(private serv: TeamsService) {}

  ngOnInit() {
    this.loadUsers();
  }
   
  //загрузка пользователей
  private loadUsers() {    
     this.teams = this.serv.getTeams();
  }

}


