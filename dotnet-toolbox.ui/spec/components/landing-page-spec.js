require('../spec-helper');

describe('PackageAdder', () => {
  beforeEach(() => {
    var LandingPage = require("../../src/js/components/landing-page.js");
    ReactDOM.render(<LandingPage/>, root);
  });

  it('loads the categories from the api', () => {
    var request = jasmine.Ajax.requests.filter(/api\/categories/).pop();
    expect(request).toBeDefined();
    expect(request.url).toBe('/api/categories');
  });

  it('loads the most recently added packages', () => {
    var request = jasmine.Ajax.requests.filter(/api\/packages/).pop();
    expect(request).toBeDefined();
    expect(request.url).toBe('/api/packages');
  });

  describe('when the categories finish loading', () => {
    beforeEach(() => {
      var request = jasmine.Ajax.requests.filter(/api\/categories/).pop();
      request.respondWith({
        status: 200,
        responseText: JSON.stringify([
          {title: 'Cool Stuff', packages: [{id: 'Cool Logging'}, {id: 'Cool Bcrypt'}]},
          {title: 'Neat Stuff', packages: [{id: 'Neat Logging'}, {id: 'Neat Gzip'}]},
          {title: 'Rad Stuff', packages: [{id: 'The Only Rad Stuff. We should go make more rad stuff.'}]}
        ])
      });
    });

    it('renders a list of each category', () => {
      expect($('#root .category h3').eq(0).text()).toEqual('Cool Stuff');
      expect($('#root .category h3').eq(1).text()).toEqual('Neat Stuff');
      expect($('#root .category h3').eq(2).text()).toEqual('Rad Stuff');
    });

    it('renders tiles for each package in each category', () => {
      expect($('#root .category:eq(0) .package:eq(0)').text()).toEqual('Cool Logging');
      expect($('#root .category:eq(1) .package:eq(1)').text()).toEqual('Neat Gzip');
      expect($('#root .category:eq(2) .package:eq(0)').text()).toContain('The Only Rad Stuff');
    });
  });

  describe('when the recent packages request completes', () => {
    beforeEach(() => {
      var request = jasmine.Ajax.requests.filter(/api\/packages/).pop();
      request.respondWith({
        status: 200,
        responseText: JSON.stringify([{id: 'Recent Radness'}, {id: 'New Hotness'}])
      });
    });

    it('shows the recent packages as another category', () => {
      expect($('#root .category h3').eq(0).text()).toEqual('Recently Added');
      expect($('#root .category:eq(0) .package:eq(0)').text()).toContain('Recent Radness');
    });
  });
});