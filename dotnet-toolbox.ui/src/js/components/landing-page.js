var React = require('react');
var { Link } = require('react-router');
var $ = require('jquery');
var PackageAdder = require('./package-adder');
var Divider = require('pui-react-dividers').Divider;
var LeaderPanel = require('./layout').LeaderPanel;
var {Row, Col} = require('pui-react-grids');
var Panel = require('pui-react-panels').Panel;

var PackageTile = React.createClass({
  render() {
    return (
      <Col md={4}>
        <Link to={"/package/" + this.props.package.id}>
          <Panel className="bg-accent-6 type-dark-1">
            <h4 className="package">{this.props.package.id}</h4>
            <p><strong>current version: </strong> {this.props.package.version }</p>
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

var LandingPage =
  React.createClass({
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
        </div>
      )
    }
  });

export default LandingPage;