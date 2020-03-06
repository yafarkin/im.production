import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GameConfigDto } from '../models/game.config.dto';

@Injectable()
export class GameManagementService {

    private gameConfigUrl: string = "/api/game/get-game-config";
    private currentDayUrl: string = "/api/game/get-current-day";
    private calculateDayUrl: string = "/api/game/calculate";
    private restartGameUrl: string = "/api/game/restart";

    constructor(private httpClient: HttpClient) {
    }

    getGameConfig(): Observable<GameConfigDto> {
        return this.httpClient.get<GameConfigDto>(this.gameConfigUrl);
    }

    getCurrentDay(): Observable<number> {
        return this.httpClient.get<number>(this.currentDayUrl);
    }

    calculateDay() {
        return this.httpClient.put(this.calculateDayUrl, {});
    }

    restartGame() {
        return this.httpClient.put(this.restartGameUrl, {});
    }
}
