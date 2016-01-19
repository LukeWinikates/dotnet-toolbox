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
    beforeEach(()=> {
      $('#package-name').val('fizzbuzz-enterprise').simulate('change');
      $('#root form').simulate('submit');
      request = jasmine.Ajax.requests.mostRecent();
    });

    it('shows a spinner', () => {
      expect($('#root marquee')).toExist();
    });

    it('disables the button', () => {
        expect($('#root button')).toBeDisabled();
    });

    it('disables the input', () => {
        expect($('#package-name')).toHaveAttr('readonly');
    });

    it('posts to the api backend', () => {
      expect(request.url).toBe('/api/packages');
      expect(request.data()).toEqual({name: 'fizzbuzz-enterprise'});
    });

    describe('when the request fails', () => {
      beforeEach(() => {
        request.respondWith({status: 404});
      });

      it('remembers that there was an error', () => {
        expect($("#root .alert-danger")).toExist();
      });

      it('hides the spinner', () => {
        expect($('#root marquee')).not.toExist();
      });
    });

    describe('when the request succeeds', () => {
      beforeEach(() => {
        request.respondWith({status: 204});
      });

      it('remembers that the request succeeded', () => {
        expect($("#root .alert-success")).toExist();
      });

      it('hides the spinner', () => {
        expect($('#root marquee')).not.toExist();
      });

      describe('when the text changes again', () => {
        beforeEach(() => {
          $('#package-name').val('fizzbuzz-yagni').simulate('change');
        });
        it('clears the existing feedback', () => {
          expect($("#root .alert-success")).not.toExist();
        });
      });
    });
  });
});
