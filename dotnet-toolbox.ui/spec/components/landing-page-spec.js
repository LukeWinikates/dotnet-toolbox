require('../spec-helper');

describe('PackageAdder', () => {
  beforeEach(() => {
    var LandingPage = require("../../src/js/components/landing-page.js");
    ReactDOM.render(<LandingPage/>, root);
  });

  it('loads the categories from the api', () => {
    var request = jasmine.Ajax.requests.mostRecent();
    expect(request).toBeDefined();
    expect(request.url).toBe('/api/categories');
  });

  describe('when the categories finish loading', () => {
    it('renders a list of each category', () => {
      var request = jasmine.Ajax.requests.mostRecent();
      request.respondWith({
        status: 200,
        responseText: JSON.stringify([
          {title: 'Cool Stuff'},
          {title: 'Neat Stuff'},
          {title: 'Rad Stuff'}
        ])
      });

      expect($('#root .category').eq(0).text()).toEqual('Cool Stuff');
      expect($('#root .category').eq(1).text()).toEqual('Neat Stuff');
      expect($('#root .category').eq(2).text()).toEqual('Rad Stuff');
    });
  });
});