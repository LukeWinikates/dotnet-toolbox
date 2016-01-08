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

  describe('submitting the form', () => {
    it('posts to the api backend', () => {
      $('#package-name').val('fizzbuzz-enterprise').simulate('change');
      $('#root form').simulate('submit');
      var request = jasmine.Ajax.requests.mostRecent();
      expect(request.url).toBe('/api/packages');
      expect(request.data()).toEqual({name: 'fizzbuzz-enterprise'});
    });
  });
});
