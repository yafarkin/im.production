import { Component } from '@angular/core';
import { OnInit } from '@angular/core';
import { Team } from '../models/team';
import { TeamsService } from '../services/teams.service';

@Component({
  selector: 'app-teams',
  templateUrl: './teams.component.html',
  styleUrls: ['./teams.component.scss'],
  providers: [TeamsService]
})

export class TeamsComponent implements OnInit {

  displayedColumns: string[] = ['name', 'productionType', 'factories', 'sum', 'contracts'];
  teams: Team[];

  constructor(private serv: TeamsService) { }

  ngOnInit() {
    this.loadUsers();
  }

  private loadUsers() {
    this.teams = this.serv.getTeams();
  }

}


