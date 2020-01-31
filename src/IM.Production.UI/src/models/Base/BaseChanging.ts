import { GameTime } from './GameTime';
import { Customer } from '../Customer/Customer';

export class BaseChanging {
    constructor(
        /// <summary>
        /// Когда произошли изменения.
        /// </summary>
        public time: GameTime,
        /// <summary>
        /// С какой командой связаны изменения.
        /// </summary>
        public customer: Customer, 
        /// <summary>
        /// Текстовое описание для UI о сути изменений.
        /// </summary>
        public description: string = null)
    {
    }
}