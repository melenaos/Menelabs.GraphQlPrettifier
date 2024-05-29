
using Menelabs;

namespace TestProject
{
    public class GraphQlPrettifierUnitTest
    {
        [Fact]
        public void SimpleQuery()
        {
            var prettifier = ProvidePrettifier();
            var pretty = prettifier.Prettify("query{order(id:\"gid:unique_id\"){name}}");

            Assert.Equal(@"query {
  order(id: ""gid:unique_id"") {
    name
  }
}",
pretty);
        }

        [Fact]
        public void MultipleFields()
        {
            var prettifier = ProvidePrettifier();
            var pretty = prettifier.Prettify("query{order(id:\"gid:unique_id\"){name id code}}");

            Assert.Equal(@"query {
  order(id: ""gid:unique_id"") {
    name
    id
    code
  }
}",
pretty);
        }

        [Fact]
        public void NestedObjects()
        {
            var prettifier = ProvidePrettifier();
            var pretty = prettifier.Prettify("query{order(id:\"gid:unique_id\"){name id image{width height url}}}");

            Assert.Equal(@"query {
  order(id: ""gid:unique_id"") {
    name
    id
    image {
      width
      height
      url
    }
  }
}",
pretty);
        }

        [Fact]
        public void NestedObjectsWithArguments()
        {
            var prettifier = ProvidePrettifier();
            var pretty = prettifier.Prettify("query{order(id:\"gid:unique_id\"){name id image(size:BIG,maxWidth:1000){width height url}product title}}");

            Assert.Equal(@"query {
  order(id: ""gid:unique_id"") {
    name
    id
    image(size: BIG, maxWidth: 1000) {
      width
      height
      url
    }
    product
    title
  }
}",
pretty);
        }

        [Fact]
        public void NestedObjectsWithObjectArguments()
        {
            var prettifier = ProvidePrettifier();
            var pretty = prettifier.Prettify("query{order(id:\"gid:unique_id\"){name id image(transformation:{size:BIG,maxWidth:1000}){width height url}product title}}");

            Assert.Equal(@"query {
  order(id: ""gid:unique_id"") {
    name
    id
    image(transformation: {size: BIG, maxWidth: 1000}) {
      width
      height
      url
    }
    product
    title
  }
}",
pretty);
        }

        [Fact]
        public void MultipleArgs()
        {
            var prettifier = ProvidePrettifier();
            var pretty = prettifier.Prettify("query{order(id:\"gid:unique_id\",max:5,min:2){name id code}}");

            Assert.Equal(@"query {
  order(id: ""gid:unique_id"", max: 5, min: 2) {
    name
    id
    code
  }
}",
pretty);
        }

        private IPrettifyGraphQl ProvidePrettifier()
        {
            return new GraphQlPrettifier();
        }
    }
}