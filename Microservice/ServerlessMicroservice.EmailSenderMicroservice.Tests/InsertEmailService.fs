module InsertEmailService

open Xunit

[<Fact>]
let ``When inserting email it should invoke S3 Client to upload file``() =
    Assert.True(false)

[<Fact>]
let ``When inserting email it should send message to SQS Client``() =
    Assert.True(false)

[<Fact>]
let ``When inserting email it should throw exception when didn't upload to S3 with success``() =
    Assert.True(false)
