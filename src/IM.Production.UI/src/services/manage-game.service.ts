import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GameConfigDto } from '../models/dtos/game.config.dto';

@Injectable()
export class ManageGameService {

    private gameConfigUrl: string = "/api/game/get-game-config";
    private calculateDayUrl: string = "/api/game/calculate";
    private restartGameUrl: string = "/api/game/restart";

    constructor(private httpClient: HttpClient) {
    }

    getGameConfig(): Observable<GameConfigDto> {
        return this.httpClient.get<GameConfigDto>(this.gameConfigUrl);
    }

    calculateDay(): Observable<number> {
        return this.httpClient.get<number>(this.calculateDayUrl);
    }

    restartGame() {
        return this.httpClient.get(this.restartGameUrl);
    }
}
