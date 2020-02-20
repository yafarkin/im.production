import { Component, OnInit } from '@angular/core';
import { ManageGameService } from '../services/manage-game.service';
import { GameConfigDto } from '../models/dtos/game.config.dto';

enum GameState {
    Initilized,
    Stopped,
    Launched,
    Finished
}

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

    gameState: GameState;

    constructor(private manageGameService: ManageGameService) { }

    ngOnInit() {
        this.progressBarValue = 0;
        this.gameState = GameState.Initilized;
        this.updateConfig();
    }

    stopGame(): void {
        this.stopGameInterval();
        this.playIntervalId = null;
        this.secondsLeftIntervalId = null;

        this.gameState = GameState.Stopped;
    }

    playGame(): void {
        if (this.gameState != GameState.Launched) {
            this.updateConfig();

            if (this.gameState == GameState.Initilized) {
                this.secondsBeforeNextDay = this.gameConfig.dayDurationInSeconds;
            }

            this.gameState = GameState.Launched;
            this.percentCoefficient = 100 / this.gameConfig.dayDurationInSeconds;

            this.playIntervalId = setInterval(() => {
                this.manageGameService.calculateDay().subscribe(
                    success => { this.currentDay = success; }
                );
                this.secondsBeforeNextDay = this.gameConfig.dayDurationInSeconds;
            }, this.gameConfig.dayDurationInSeconds * 1000);

            this.secondsLeftIntervalId = setInterval(() => {
                if (this.secondsBeforeNextDay > 0) {
                    this.secondsBeforeNextDay--;
                }
                this.progressBarValue = this.percentCoefficient *
                (this.gameConfig.dayDurationInSeconds - this.secondsBeforeNextDay);

                if (this.currentDay == this.gameConfig.maxDays &&
                    this.secondsBeforeNextDay == 0)
                {
                    this.gameState = GameState.Finished;
                    this.stopGameInterval();
                }
            }, 1000);
        }
    }

    updateConfig(): void {
        this.manageGameService.getGameConfig().subscribe(
            success => {
                this.gameConfig = success;
            }
        );
    }

    restartGame(): void {
        this.stopGameInterval();
        this.updateConfig();
        this.manageGameService.restartGame().subscribe(
            success => {
                this.currentDay = 0;
                this.progressBarValue = 0;
                this.secondsBeforeNextDay = this.gameConfig.dayDurationInSeconds;
                this.gameState = GameState.Initilized;
            }
        );
    }

    stopGameInterval(): void {
        clearInterval(this.playIntervalId);
        clearInterval(this.secondsLeftIntervalId);
    }

    isGameInitialized(): boolean {
        return this.gameState == GameState.Initilized;
    }

    isGameStopped(): boolean {
        return this.gameState == GameState.Stopped;
    }

    isGameLaunched(): boolean {
        return this.gameState == GameState.Launched;
    }

    isGameFinished(): boolean {
        return this.gameState == GameState.Finished;
    }
}
