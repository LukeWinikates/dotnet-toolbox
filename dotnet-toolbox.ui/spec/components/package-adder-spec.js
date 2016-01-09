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
    var request;
    beforeEach(()=>{
      $('#package-name').val('fizzbuzz-enterprise').simulate('change');
      $('#root form').simulate('submit');
      request = jasmine.Ajax.requests.mostRecent();
    });

    it('posts to the api backend', () => {
      expect(request.url).toBe('/api/packages');
      expect(request.data()).toEqual({name: 'fizzbuzz-enterprise'});
    });

    describe('when the request fails', () => {
      it('remembers that there was an error', () => {
        request.respondWith({status: 404});
        expect($("#root .alert-danger")).toExist();
      });
    });

    describe('when the request succeeds', () => {
      it('remembers that the request succeeded', () => {
        request.respondWith({status: 204});
        expect($("#root .alert-success")).toExist();
      });
    });
  });
});
