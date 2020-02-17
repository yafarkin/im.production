import { Component, OnInit } from '@angular/core';
import { ManageGameService } from '../services/manage-game.service';
import { GameConfigDto } from '../models/dtos/game.config.dto';

@Component({
    selector: 'app-manage-game',
    templateUrl: './manage-game.component.html',
    styleUrls: ['./manage-game.component.scss'],
    providers: [ManageGameService]
})
export class ManageGameComponent implements OnInit {

    currentDay: number = 0;
    gameConfig: GameConfigDto = new GameConfigDto();
    playIntervalId;
    secondsLeftIntervalId;
    secondsBeforeNextDay: number;
    progressBarValue: number;
    percentCoefficient: number;
    constructor(private manageGameService: ManageGameService) { }

    ngOnInit() {
        this.progressBarValue = 0;
        this.manageGameService.getGameConfig().subscribe(
            success => {
                this.gameConfig = success;
            }
        );
    }

    stopGame(): void {
        clearInterval(this.playIntervalId);
        clearInterval(this.secondsLeftIntervalId);
        this.playIntervalId = null;
        this.secondsLeftIntervalId = null;
        this.secondsBeforeNextDay = 0;
    }

    playGame(): void {
        this.percentCoefficient = 100 / this.gameConfig.dayDurationInSeconds;
        this.secondsBeforeNextDay = this.gameConfig.dayDurationInSeconds;
        this.playIntervalId = setInterval(() => {
            this.manageGameService.calculateDay().subscribe(
                success => {
                    this.currentDay = success;
                }
            );
            this.secondsBeforeNextDay =this.gameConfig.dayDurationInSeconds;
            }, this.gameConfig.dayDurationInSeconds * 1000);

        this.secondsLeftIntervalId = setInterval(() => {
            if (this.secondsBeforeNextDay > 0)
            this.secondsBeforeNextDay--;
            this.progressBarValue = this.percentCoefficient *
                (this.gameConfig.dayDurationInSeconds - this.secondsBeforeNextDay);
        }, 1000);
    }

    restartGame(): void {
        this.manageGameService.restartGame().subscribe(
            success => {
                this.currentDay = 0;
                this.progressBarValue = 0;
            }
        );
    }
}
