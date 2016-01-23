require('../spec-helper');

describe('package-page', () => {
  beforeEach(() => {
    var PackagePage = require("../../src/js/components/package-page.js");
    ReactDOM.render(<PackagePage params={{packageName:"Bread.Oven"}}/>, root);
  });

  it('loads the package from the api', () => {
    var request = jasmine.Ajax.requests.mostRecent();
    expect(request).toBeDefined();
    expect(request.url).toBe('/api/packages/Bread.Oven');
  });

  describe('when loading the package succeeds', () => {
    it('shows the package details', () => {
      jasmine.Ajax.requests.mostRecent()
        .respondWith({status: 200, responseText: JSON.stringify({owners: 'Yeast'})});

      expect($('#root').text()).toContain('Yeast');
    });
  });
});