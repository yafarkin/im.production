import { Component, OnInit } from '@angular/core';
import { GameManagementService } from '../services/game.management.service';
import { interval, Subscription, timer } from 'rxjs';
import { GameConfigDto } from '../models/dtos/game.config.dto';

enum GameState {
    Initilized,
    Stopped,
    Launched,
    Finished
}

@Component({
    selector: 'app-game-management',
    templateUrl: './game-management.component.html',
    styleUrls: ['./game-management.component.scss'],
    providers: [GameManagementService]
})
export class GameManagementComponent implements OnInit {

    currentDay: number = 0;
    gameConfig: GameConfigDto;
    playIntervalId: Subscription;
    secondsBeforeNextDay: number;
    secondsLeft: number;
    progressBarValue: number;
    percentCoefficient: number;

    gameState: GameState;

    constructor(private gameManagementService: GameManagementService) { }

    ngOnInit() {
        this.initializeGame();
    }

    stopGame(): void {
        this.stopGameInterval();
        this.gameState = GameState.Stopped;
    }

    playGame(): void {
        if (this.gameState != GameState.Launched) {
            if (this.gameState == GameState.Initilized) {
                this.secondsBeforeNextDay = this.gameConfig.dayDurationInSeconds;
            }

            this.gameState = GameState.Launched;
            this.percentCoefficient = 100 / this.gameConfig.dayDurationInSeconds;

            this.playIntervalId = interval(1000)
                .subscribe(
                    useless => {
                        ++this.secondsLeft;
                        if (this.secondsBeforeNextDay > 0) {
                            this.secondsBeforeNextDay = this.gameConfig.dayDurationInSeconds - this.secondsLeft;
                            if (this.secondsBeforeNextDay == 0
                                && this.currentDay != this.gameConfig.totalDays) {
                                this.gameManagementService.calculateDay().subscribe(success => {
                                        this.gameManagementService.getCurrentDay().subscribe(currentDay => {
                                                this.secondsLeft = 0;
                                                this.progressBarValue = 0;
                                                this.secondsBeforeNextDay = this.gameConfig.dayDurationInSeconds;
                                                this.currentDay = currentDay;
                                            }
                                        )
                                    }
                                );
                            }
                        }

                        this.progressBarValue = this.percentCoefficient *
                            (this.gameConfig.dayDurationInSeconds - this.secondsBeforeNextDay);

                        if (this.currentDay == this.gameConfig.totalDays
                            && this.secondsBeforeNextDay == 0) {
                            this.gameState = GameState.Finished;
                            this.stopGameInterval();
                        }
                    }
                );

        }
    }

    updateConfig(): void {
        this.gameManagementService.getGameConfig().subscribe(success => {
                this.gameConfig = success;
                this.secondsBeforeNextDay = success.dayDurationInSeconds;
            }
        );
    }

    initializeGame(): void {
        this.secondsLeft = 0;
        this.currentDay = 0;
        this.progressBarValue = 0;
        this.gameState = GameState.Initilized;
        this.updateConfig();
        this.gameManagementService.getCurrentDay().subscribe(currentDay => {
            this.currentDay = currentDay;
        });
    }

    restartGame(): void {
        this.stopGameInterval();
        this.gameManagementService.restartGame().subscribe(
            success => {
                this.initializeGame();
            }
        );
    }

    stopGameInterval(): void {
        if (this.playIntervalId) {
            this.playIntervalId.unsubscribe();
        }
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
