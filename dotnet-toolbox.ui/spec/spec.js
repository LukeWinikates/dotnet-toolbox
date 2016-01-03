var fizz = require("../src/fizz.js");

describe('stuff', () => {
    it('is there', () => {
        expect(fizz.buzz()).toBe(2);
    });
});
