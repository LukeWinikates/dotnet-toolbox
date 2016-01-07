require('../spec-helper');

describe('PackageAdder', () => {
  beforeEach(() => {
    var PackageAdder = require("../../src/js/components/package-adder.js");
    ReactDOM.render(<PackageAdder/>, root);
  });

  describe('the add button', () => {
    it('is disabled unless the input has content', () => {
      expect("#root button.btn").toBeDisabled();
      $('#package-name').val('fizzbuzz').simulate('change');
      expect("#root button.btn").not.toBeDisabled();
    });
  });
});
