import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Team } from '../models/team';

@Injectable()
export class TeamsService {
    /*private url = "http://localhost:63333/api/teams";
    constructor(private http: HttpClient){ }*/

    getTeams() {
        let testteam1: Team = new Team("AAA", "metal", "testFactory", 12338, "testContract");
        let testteam2: Team = new Team("BBB", "chemical", "testFactory", 24234, "testContract");
        let testteam3: Team = new Team("CCC", "metall", "testFactory", 9772, "testContract");
        let testteam4: Team = new Team("DDD", "electronic", "testFactory", 6745, "testContract");
        let testteams: Team[] = [testteam1, testteam2, testteam3, testteam4];
        return testteams;
    }
}
