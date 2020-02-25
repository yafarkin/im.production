import { UserDto } from './user.dto';

describe('User', () => {
    it('should create an instance', () => {
        expect(new UserDto()).toBeTruthy();
    });
});
