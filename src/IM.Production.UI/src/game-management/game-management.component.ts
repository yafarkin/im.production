import { Component, OnInit } from '@angular/core';
import { GameManagementService } from '../services/game.management.service';
import { interval, Subscription } from 'rxjs';
import { GameConfigDto } from '../models/dtos/game.config.dto';

enum GameState {
    Initilized,
    Stopped,
    Launched,
    Finished
}

const ONE_SECOND: number = 1000;

@Component({
    selector: 'app-game-management',
    templateUrl: './game-management.component.html',
    styleUrls: ['./game-management.component.scss'],
    providers: [GameManagementService]
})
export class GameManagementComponent implements OnInit {

    gameConfig: GameConfigDto;
    gameState: GameState;
    playIntervalSubscription: Subscription;
    currentDay: number;
    secondsBeforeNextDay: number;
    secondsAfterPreviousDay: number;
    progressBarValue: number;
    percentCoefficient: number;

    constructor(private gameManagementService: GameManagementService) { }

    ngOnInit() {
        this.initializeGame();
    }

    initializeGame(): void {
        this.gameState = GameState.Initilized;
        this.resetProgression();
        this.updateConfig();
        this.updateCurrentDay();
    }

    stopGame(): void {
        this.gameState = GameState.Stopped;
        this.stopPlayInterval();
    }

    playGame(): void {
        if (this.gameState == GameState.Launched) {
            return;
        }

        if (this.gameState == GameState.Initilized) {
            this.secondsBeforeNextDay = this.gameConfig.dayDurationInSeconds;
        }

        this.gameState = GameState.Launched;
        this.percentCoefficient = 100 / this.gameConfig.dayDurationInSeconds;

        this.startPlayInterval();
    }

    restartGame(): void {
        this.stopPlayInterval();
        this.gameManagementService.restartGame().subscribe(
            success => {
                this.initializeGame();
            }
        );
    }

    finishGame(): void {
        this.gameState = GameState.Finished;
        this.stopPlayInterval();
    }

    calculateNextDay(): void {
        this.gameManagementService.calculateDay().subscribe(
            success => {
                this.secondsBeforeNextDay = this.gameConfig.dayDurationInSeconds;
                this.updateCurrentDay();
                this.resetProgression();
            }
        );
    }

    startPlayInterval(): void {
        this.playIntervalSubscription = interval(ONE_SECOND).subscribe(
        useless => {
            ++this.secondsAfterPreviousDay;

            this.secondsBeforeNextDay =
                this.gameConfig.dayDurationInSeconds - this.secondsAfterPreviousDay;
            this.progressBarValue = this.percentCoefficient *
                (this.gameConfig.dayDurationInSeconds - this.secondsBeforeNextDay);

            if (this.isEndOfLastDay(this.currentDay)) {
                this.finishGame();
            }
            else if (this.isEndOfDay(this.currentDay)) {
                this.calculateNextDay();
            }
        });
    }

    stopPlayInterval(): void {
        if (this.playIntervalSubscription) {
            this.playIntervalSubscription.unsubscribe();
        }
    }

    updateCurrentDay(): void {
        this.gameManagementService.getCurrentDay().subscribe(currentDay => {
            this.currentDay = currentDay;
        });
    }

    updateConfig(): void {
        this.gameManagementService.getGameConfig().subscribe(success => {
                this.gameConfig = success;
                this.secondsBeforeNextDay = success.dayDurationInSeconds;
            }
        );
    }

    resetProgression(): void {
        this.secondsAfterPreviousDay = 0;
        this.progressBarValue = 0;
    }

    isEndOfDay(currentDay: number): boolean {
        return currentDay != this.gameConfig.totalDays && this.secondsBeforeNextDay == 0;
    }

    isEndOfLastDay(currentDay: number): boolean {
        return currentDay == this.gameConfig.totalDays && this.secondsBeforeNextDay == 0;
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
