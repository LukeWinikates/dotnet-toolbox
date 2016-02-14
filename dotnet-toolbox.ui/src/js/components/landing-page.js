var React = require('react');
var { Link } = require('react-router');
var $ = require('jquery');
var PackageAdder = require('./package-adder');
var Divider = require('pui-react-dividers').Divider;
var LeaderPanel = require('./layout').LeaderPanel;
var {Row, Col} = require('pui-react-grids');
var Panel = require('pui-react-panels').Panel;
var {FormattedNumber} = require('react-intl');

var PackageTile = React.createClass({
  render() {
    return (
      <Col md={4} className="package-tile">
        <Link to={"/package/" + this.props.package.id}>
          <Panel className="bg-accent-6 type-dark-1">
            <h4 className="package">{this.props.package.id}</h4>
            <p><strong>current version: </strong> {this.props.package.version }</p>
            <p><strong>total downloads: </strong>
              {
                (() => {
                  if (this.props.package.totalDownloads) {
                    return (<FormattedNumber value={this.props.package.totalDownloads}/>)
                  }
                  else {
                    return (<span>not known</span>)
                  }
                })()
              }
            </p>
            <em>{this.props.package.description }</em>
          </Panel>
        </Link>
      </Col>
    )
  }
});

var Category = React.createClass({
  render() {
    return (
      <section>
        <Divider/>
        <article className="container category">
          <h3>{this.props.category.title}</h3>
          <Row>
            {
              this.props.category.packages.map(p => {
                return (<PackageTile key={p.id} package={p}/>)
              })
            }
          </Row>
        </article>
      </section>
    )
  }
});

var RecentlyAddedCategory = React.createClass({
  componentDidMount() {
    $.ajax({
      type: 'get',
      url: '/api/packages',
      contentType: 'application/json'
    }).then(packages => {
      this.setState({packages: packages});
    });
  },
  getInitialState() {
    return {};
  },
  render() {
    if (this.state.packages) {
      return (<Category category={{title: "Recently Added", packages: this.state.packages}}/>);
    }
    return (<marquee>...</marquee>);
  }
});

var LandingPage =
  React.createClass({
    getInitialState() {
      return {}
    },
    componentDidMount() {
      $.ajax({
        type: 'get',
        url: '/api/categories',
        contentType: 'application/json'
      }).then(categories => {
        this.setState({categories: categories})
      });
    },
    render() {
      return (
        <div>
          <LeaderPanel>
            <PackageAdder/>
          </LeaderPanel>
          <h2 className="container">Libraries by Category</h2>
          {
            this.state && this.state.categories && this.state.categories.map((c)=> {
              return (<Category key={c.title} category={c}/>)
            })
          }
          <RecentlyAddedCategory/>
        </div>
      )
    }
  });

export default LandingPage;