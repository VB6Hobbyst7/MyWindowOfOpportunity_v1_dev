Imports umbraco
Imports umbraco.Core
Imports umbraco.Core.Models
Imports umbraco.Core.Services
Imports umbraco.NodeFactory
Imports umbraco.Web
Imports System.Security.Cryptography
Imports System.IO
Imports System.Net
Imports System.Xml.XPath
Imports System.Net.Mail

Public NotInheritable Class Common

#Region "Properties"
    Public Enum siteNodes
        AboutUs = 6426
        accountHooks = 15454
        acctCharges = 15459
        acctTransfers = 15460
        acctPayouts = 15461
        acctGeneric = 15462

        BecomeAMember = 1115

        connectHooks = 15453
        connectCharges = 15455
        connectTransfers = 15456
        connectPayouts = 15457
        connectGeneric = 15458

        Campaign = 4397
        CampaignManagement = 7159
        Campaigns = 1058
        CannotCreateAcct = 1665
        Checkout = 1294
        ClientMessages = 3361
        ContactUs = 2074
        CreateACampaign = 1313
        CreditCardManager = 1884
        EditAccount = 1767
        EditCampaign = 1388
        ExpiredInvitation = 1658
        failedPayouts = 15037
        FAQs = 1355
        FinancialManager = 1873
        ForgotPassword = 1876
        Help = 7148
        Home = 1048
        Invest = 4398
        Legal = 7153
        Login = 1951
        ManageCampaigns = 1314
        Market = 4399
        MemberManagement = 7158
        Messages = 1659
        pendingPayouts = 15035
        PledgesToProcess = 14993
        PreAccountMembers = 2003
        ResetPassword = 2142
        SiteErrors = 1894
        successfulPayouts = 15036
        'VerifiedSuccessfully = 15958
    End Enum
    Public Enum prevalues
        CategoryList = 1156
        Dropdown_MonthsAbreviated = 1284
        Dropdown_Years = 1285
        ImageCropper = 1043
        Subcategories_Artistic = 1162
        Subcategories_Business = 1160
        Subcategories_Charity = 1138
        Subcategories_Community = 1157
        Subcategories_Science = 1159
        Subcategories_SelfHelp = 1161
        Subcategories_Software = 1158
        Subcategories_Technology = 1145
    End Enum
    Public Enum mediaNodes
        Campaigns = 1317
        defaultProfileImg = 1696
        Members = 1637
        logoIcon_FullSize = 15463
    End Enum
    Public Enum siteValues
        campaignLength = 30
    End Enum
    Public Enum mediaType_Values
        Facebook
        Twitter
        GooglePlus
        LinkedIn
        SupportEmail
        none
    End Enum
    Public Enum statusType
        none
        AccountSetup
        Unpublished
        DiscoveryPhase
        Phase1Active
        Phase2Active
        Phase3Active
        Phase1Failed
        Phase2Failed
        Phase3Failed
        Phase1Pending
        Phase2Pending
        Phase3Pending
        Phase1Succeeded
        Phase2Succeeded
        Phase3Succeeded
        Complete
        'TBD
        'DiscoveryActive
    End Enum
    Public Enum colorSchemePercentages
        na
        bgcolor_10percent
        bgcolor_20percent
        bgcolor_30percent
        bgcolor_40percent
        bgcolor_50percent
        bgcolor_60percent
        bgcolor_70percent
        bgcolor_80percent
        bgcolor_90percent
        bgcolor_100percent
    End Enum


    Public Structure nodeProperties
        Const accessToken As String = "accessToken"
        Const acctCreated As String = "acctCreated"
        Const activationDate As String = "activationDate"
        Const activePhase As String = "activePhase"
        Const addedDate As String = "addedDate"
        Const address01 As String = "address01"
        Const address01_Billing As String = "address01_Billing"
        Const address02_Billing As String = "address02_Billing"
        Const address01_Shipping As String = "address01_Shipping"
        Const address02_Shipping As String = "address02_Shipping"
        Const address02 As String = "address02"
        Const administratorNotes As String = "administratorNotes"
        Const alternativeEmail As String = "alternativeEmail"
        Const answer As String = "answer"
        Const associatedTo As String = "associatedTo"
        Const available As String = "available"

        Const balanceTransactionId As String = "balanceTransactionId"
        Const bankAccountId As String = "bankAccountId"
        Const bankAccountToken As String = "bankAccountToken"
        Const briefDescription As String = "briefDescription"
        Const briefSummary As String = "briefSummary"
        Const bookmarkContent As String = "content"

        Const campaign As String = "campaign"
        Const campaignAccountId As String = "campaignAccountId"
        Const campaignComplete As String = "campaignComplete"
        Const campaignImages As String = "Campaign Images"
        Const campaignManager As String = "campaignManager"
        Const campaignManagers As String = "campaignManagers"
        Const campaignMngrNotes As String = "campaignMngrNotes"
        Const campaignMember As String = "campaignMember"
        Const canceled As String = "canceled"
        Const category01 As String = "category01"
        Const category01Icon As String = "category01Icon"
        Const category02 As String = "category02"
        Const category02Icon As String = "category02Icon"
        Const category03 As String = "category03"
        Const category03Icon As String = "category03Icon"
        Const category04 As String = "category04"
        Const category04Icon As String = "category04Icon"
        Const category05 As String = "category05"
        Const category05Icon As String = "category05Icon"
        Const category06 As String = "category06"
        Const category06Icon As String = "category06Icon"
        Const category07 As String = "category07"
        Const category07Icon As String = "category07Icon"
        Const category08 As String = "category08"
        Const category08Icon As String = "category08Icon"
        Const category09 As String = "category09"
        Const category09Icon As String = "category09Icon"
        Const categoryDescription As String = "categoryDescription"
        Const chargeId As String = "chargeId"
        Const chargeStatus As String = "chargeStatus"
        Const city_Billing As String = "city_Billing"
        Const city_Shipping As String = "city_Shipping"
        Const city_company As String = "city_company"
        Const claimed As String = "claimed"
        Const companyName As String = "companyName"
        Const completionContent As String = "completionContent"
        Const completionDate As String = "completionDate"
        Const contributionAmount As String = "contributionAmount"
        Const contribution_Amount As String = "Contribution Amount"
        Const copyrightTitle As String = "copyrightTitle"
        Const creationDate As String = "creationDate"
        Const creditCardId As String = "creditCardId"
        Const creditCardToken As String = "creditCardToken"
        Const customCSS As String = "customCSS"
        Const customerId As String = "customerId"

        Const dateCanceled As String = "dateCanceled"
        Const dateDeclined As String = "dateDeclined"
        Const dateOfBirth As String = "dateOfBirth"
        Const datePublished As String = "datePublished"
        Const dateToProcess As String = "dateToProcess"
        Const description As String = "description"
        Const destinationAccount As String = "destinationAccount"
        Const destinationId As String = "destinationId"

        Const email As String = "email"
        Const entryDate As String = "entryDate"
        Const Entry_Date As String = "Entry Date"
        Const errorMessage As String = "errorMessage"
        Const estimatedShippingMonth As String = "estimatedShippingMonth"
        Const estimatedShippingYear As String = "estimatedShippingYear"
        Const exceptionMessage As String = "exceptionMessage"

        Const failedDate As String = "failedDate"
        Const fax_company As String = "fax_company"
        Const facebookUrl As String = "facebookUrl"
        Const featuredImage As String = "featuredImage"
        Const fileUploadId As String = "fileUploadId"
        Const firstName As String = "firstName"
        Const fulfilled As String = "fulfilled"
        Const fulfillmentDate As String = "fulfillmentDate"
        Const fullSummary As String = "fullSummary"
        Const FAQs As String = "faqs"
        Const Faq As String = "fAQ"

        Const generalMessage As String = "generalMessage"
        Const goal As String = "goal"
        Const googlePlusUrl As String = "googlePlusUrl"

        Const heading As String = "heading"
        Const homeBanner As String = "homeBanner"
        Const hookMessage As String = "hookMessage"
        Const howThisWorksVideo As String = "howThisWorksVideo"

        Const introductionContent As String = "introductionContent"
        Const isComplete As String = "isComplete"
        Const isFacebookAcct As String = "isFacebookAcct"
        Const isLinkedInAcct As String = "isLinkedInAcct"
        Const isTwitterAcct As String = "isTwitterAcct"

        Const keepInformed As String = "keepInformed"

        Const lastName As String = "lastName"
        Const linkedInUrl As String = "linkedInUrl"

        Const mediaFolder As String = "mediaFolder"
        Const member As String = "member"
        Const message As String = "message"
        Const mWoOFees As String = "mWoOFees"
        'Const menuDescription As String = "description"
        'Const menuTitle As String = "title"

        Const Name As String = "Name"
        Const navigateTo As String = "navigateTo"
        Const netPledgeAmount As String = "netPledgeAmount"
        Const noFeesApplied As String = "noFeesApplied"
        Const notes As String = "notes"
        Const notesFrontEnd As String = "notesFrontEnd"
        Const notesBackEnd As String = "notesBackEnd"

        Const password As String = "password"
        Const paymentId As String = "paymentId"
        Const payoutTotal As String = "payoutTotal"
        Const personName As String = "personname"
        Const phaseActive As String = "phaseActive"
        Const phaseComplete As String = "phaseComplete"
        Const phaseFailed As String = "phaseFailed"
        Const phaseDescription As String = "phaseDescription"
        Const phaseNumber As String = "phaseNumber"
        Const photo As String = "photo"
        Const phoneNumber As String = "phoneNumber"
        Const phone_company As String = "phone_company"
        Const pledgeAmount As String = "pledgeAmount"
        Const pledgeDate As String = "pledgeDate"
        Const pledges As String = "pledges"
        Const pledgesToProcess As String = "pledgesToProcess"
        Const pledgesWithErrors As String = "pledgesWithErrors"
        Const pledgingMember As String = "pledgingMember"
        Const postalCode_Billing As String = "postalCode_Billing"
        Const postalCode_Shipping As String = "postalCode_Shipping"
        Const postalCode_company As String = "postalCode_company"
        Const previousPhases As String = "previousPhases"
        Const published As String = "published"

        Const question As String = "question"

        Const readOnlyMessage As String = "readOnlyMessage"
        Const receivedOn As String = "receivedOn"
        Const refreshToken As String = "refreshToken"
        Const reimbursed As String = "reimbursed"
        Const reimbursedDate As String = "reimbursedDate"
        Const reviews As String = "reviews"
        Const rewardImages As String = "Reward Images"
        Const rewardSelected As String = "rewardSelected"
        Const rewardShipped As String = "rewardShipped"
        Const role As String = "role"

        Const sampleCampaigns As String = "sampleCampaigns"
        Const shortDescription As String = "shortDescription"
        Const showAsAnonymous As String = "showAsAnonymous"
        Const showInFooter As String = "showInFooter"
        Const showInNavigation As String = "showInNavigation"
        Const showOnSide As String = "showOnSide"
        Const showOnTimeline As String = "showOnTimeline"
        Const sourceTransaction As String = "sourceTransaction"
        Const stars As String = "stars"
        Const state_company As String = "state_company"
        Const stateprovidence_Billing As String = "stateprovidence_Billing"
        Const stateprovidence_Shipping As String = "stateprovidence_Shipping"

        Const stripeFees As String = "stripeFees"
        Const stripeClientId As String = "stripeClientId"
        Const stripePublishableKey As String = "stripePublishableKey"
        Const stripeUserId As String = "stripeUserId"

        Const supportUrl As String = "supportUrl"
        Const subject As String = "subject"
        Const successMessage As String = "successMessage"
        Const summary As String = "summary"
        Const summaryPanelImage As String = "summaryPanelImage"
        Const supportEmail As String = "supportEmail"
        Const supportEmailUrl As String = "supportEmailUrl"

        Const teamAdministrators As String = "teamAdministrators"
        Const teamImage As String = "teamImage"
        Const timelineImages As String = "Timeline Images"
        Const title As String = "title"
        Const topBannerImage As String = "topBannerImage"
        Const trackingNo As String = "trackingNo"
        Const transactionDeclined As String = "transactionDeclined"
        Const transactionId As String = "transactionId"
        Const transfered As String = "transfered"
        Const transferDate As String = "transferDate"
        Const transferGroup As String = "transferGroup"
        Const transferId As String = "transferId"
        'Const transferTotal As String = "transferTotal"
        Const twitterUrl As String = "twitterUrl"

        Const _umb_email As String = "_umb_email"
        Const _umb_login As String = "_umb_login"

        Const whoAreWe As String = "whoAreWe"
    End Structure
    Public Structure docTypes
        Const accountHooks As String = "accountHooks"
        Const AlphaFolder As String = "AlphaFolder"
        Const Campaign As String = "Campaign"
        Const Campaigns As String = "Campaigns"
        Const campaignMember As String = "campaignMember"
        Const campaignMembers As String = "campaignMembers"
        Const charge As String = "charge"
        Const charges As String = "charges"
        Const contactUs As String = "contactUs"
        Const Checkout As String = "Checkout"
        Const corrected As String = "corrected"
        Const discovery As String = "discovery"
        Const editAccount As String = "editAccount"
        Const editCampaign As String = "editCampaign"
        Const errorMessage As String = "errorMessage"
        Const errors As String = "errors"
        Const failedPayouts As String = "failedPayouts"
        Const FAQ As String = "FAQ"
        Const FAQs As String = "FAQs"
        Const genericHook As String = "genericHook"
        Const genericHooks As String = "genericHooks"
        Const Home As String = "Home"
        Const ignore As String = "ignore"
        Const messageItem As String = "messageItem"
        Const navigationLink As String = "navigationLink"
        Const payout As String = "payout"
        Const payoutHook As String = "payoutHook"
        Const payoutHooks As String = "payoutHooks"
        Const payouts As String = "payouts"
        Const pendingPayouts As String = "pendingPayouts"
        Const preAccountMember As String = "preAccountMember"
        Const preAccountMembers As String = "preAccountMembers"
        Const Phase As String = "Phase"
        Const Phases As String = "Phases"
        Const Pledges As String = "pledge"
        Const PreviousPhases As String = "previousPhases"
        Const PrimarySubject As String = "PrimarySubject"
        Const review As String = "review"
        Const Reward As String = "Reward"
        Const Rewards As String = "Rewards"
        Const segment As String = "segment"
        Const StandardPage As String = "StandardPage"
        Const successfulPayouts As String = "successfulPayouts"
        Const Team As String = "Team"
        Const Timeline As String = "Timeline"
        Const TimelineEntry As String = "TimelineEntry"
        Const toAddress As String = "toAddress"
        Const transfer As String = "transfer"
        Const transfers As String = "transfers"
        Const webhooks As String = "webhooks"
    End Structure
    Public Structure mediaProperty
        Const umbracoHeight As String = "umbracoHeight"
        Const umbracoExtension As String = "umbracoExtension"
        Const umbracoFile As String = "umbracoFile"
        Const umbracoBytes As String = "umbracoBytes"
        Const umbracoWidth As String = "umbracoWidth"
    End Structure
    Public Structure classes
        Const hide As String = "hide"
    End Structure
    Public Structure Categories
        Const Categories As String = "Categories"
        Const Arts As String = "Arts"
        Const Business As String = "Business"
        Const Charity As String = "Charity"
        Const Community As String = "Community"
        Const Science As String = "Science"
        Const SelfHelp As String = "SelfHelp"
        Const Software As String = "Software"
        Const Technology As String = "Technology"
    End Structure
    Public Structure SubcategoryConverter
        Const Artistic As String = "artisticSubcategory"
        Const Business As String = "businessSubcategories"
        Const Charity As String = "charitySubcategory"
        Const Community As String = "communitySubcategories"
        Const Science As String = "scienceSubcategory"
        Const SelfHelp As String = "selfHelpSubcategory"
        Const Software As String = "softwareSubcategory"
        Const Technology As String = "technologySubcategory"
    End Structure
    Public Structure Subcategory_Artistic
        Const Art As String = "Art"
        Const Comics As String = "Comics"
        Const Crafts As String = "Crafts"
        Const Creative As String = "Creative"
        Const Dance As String = "Dance"
        Const Fashion As String = "Fashion"
        Const Food As String = "Food"
        Const Illustration As String = "Illustration"
        Const Music As String = "Music"
        Const Photography As String = "Photography"
        Const Publishing As String = "Publishing"
        Const Theater As String = "Theater"
        Const VideoFilmAndWeb As String = "Video (Film and Web)"
        Const Writing As String = "Writing"
        Const Other As String = "Other"
    End Structure
    Public Structure Subcategory_Business
        Const InternetBased As String = "Internet based"
        Const Manufacturing As String = "Manufacturing"
        Const SmallBusiness As String = "Small Business"
        Const SmallTown As String = "Small Town"
        Const Startups As String = "Startups"
        Const Other As String = "Other"
    End Structure
    Public Structure Subcategory_Charity
        Const Animals As String = "Animals"
        Const Emergencies As String = "Emergencies"
        Const Family As String = "Family"
        Const FireVictim As String = "Fire Victim"
        Const MedicalHealth As String = "Medical Health"
        Const Memorials As String = "Memorials"
        Const Moving As String = "Moving"
        Const VolunteerCenters As String = "Volunteer Centers"
        Const Other As String = "Other"
    End Structure
    Public Structure Subcategory_Community
        Const CommunityDevelopment As String = "Community Development"
        Const Environmental As String = "Environmental"
        Const Events As String = "Events"
        Const JournalismAndNews As String = "Journalism and News"
        Const PoliticalCauses As String = "Political Causes"
        Const RecreationalCenter As String = "Recreational Center"
        Const Religion As String = "Religion"
        Const Travel As String = "Travel"
        Const YouthDevelopment As String = "Youth Development"
        Const Other As String = "Other"
    End Structure
    Public Structure Subcategory_Science
        Const Archaeology As String = "Archaeology"
        Const Astronomy As String = "Astronomy"
        Const Astrophysics As String = "Astrophysics"
        Const Biology As String = "Biology"
        Const DataAnalysis As String = "Data Analysis"
        Const EarthScience As String = "Earth Science"
        Const Geology As String = "Geology"
        Const Mathematics As String = "Mathematics"
        Const MedicalResearch As String = "Medical Research"
        Const Physics As String = "Physics"
        Const Research As String = "Research"
        Const ResearchPublication As String = "Research Publication"
        Const SpaceExploration As String = "Space Exploration"
        Const Theory As String = "Theory"
    End Structure
    Public Structure Subcategory_SelfHelp
        Const Competitions As String = "Competitions"
        Const Education As String = "Education"
        Const Newlyweds As String = "Newlyweds"
        Const Sports As String = "Sports"
        Const Travel As String = "Travel"
        Const Other As String = "Other"
    End Structure
    Public Structure Subcategory_Software
        Const Browser As String = "Browser"
        Const CellPhoneApps As String = "Cell Phone Apps"
        Const ComputerApps As String = "Computer Apps"
        Const LinuxSoftware As String = "Linux Software"
        Const Programs As String = "Programs"
        Const VideoGames As String = "Video Games"
    End Structure
    Public Structure Subcategory_Technology
        Const Printer3D As String = "3D Printer"
        Const Aerospace As String = "Aerospace"
        Const Agriculture As String = "Agriculture"
        Const AstronomyHardware As String = "Astronomy Hardware"
        Const BioAndMedicine As String = "Bio and Medicine"
        Const Design As String = "Design"
        Const EnergyProduction As String = "Energy Production"
        Const Engineering As String = "Engineering"
        Const Gadgets As String = "Gadgets"
        Const GreenTech As String = "Green Tech"
        Const Materials As String = "Materials"
        Const Phone As String = "Phone"
        Const Prototype As String = "Prototype"
        Const Robotics As String = "Robotics"
        Const SpaceExploration As String = "Space Exploration"
        Const Other As String = "Other"
    End Structure
    Public Structure Miscellaneous
        'Constants without umbraco tie-ins.
        Const active As String = "active"
        Const ActiveTab As String = "ActiveTab"
        Const Anonymous As String = "Anonymous"
        Const asc As String = "asc"
        Const campaignMembers As String = "Campaign Members"
        Const campaignsToShow As UInt16 = 11
        Const clearBg As String = "clearBg"
        Const CommonPasswordForSocialLogin As String = "CommonPasswordForSocialLogin"
        Const creationDate As String = "creationDate"
        Const data As String = "data-"
        Const desc As String = "desc"
        Const disabled As String = "disabled"
        Const enterEmailAddress As String = "Enter Email Address"
        'Const feeAmount As String = "feeAmount"
        Const handler_CampaignInvitation As String = "/Services/CampaignInvitations.ashx"
        Const httpsRedirect As String = "httpsRedirect"
        Const ExceptionMessage As String = "ExceptionMessage"
        Const inactive As String = "inactive"
        Const isAdmin As String = "isAdmin"
        Const isLive As String = "isLive"
        Const isTeamName As String = "isTeamName"
        Const mediaType As String = "mediaType"
        Const MWoOFeePercent As String = "MWoOFeePercent"
        Const msgType As String = "msgType"
        Const nodeId As String = "nodeId"
        Const noFeeEnddate As String = "noFeeEnddate"
        Const noFeeStartdate As String = "noFeeStartdate"
        Const parentId As String = "parentId"
        Const PreviousPhases As String = "Previous Phases"
        Const returnObject As String = "returnObject"
        Const rewardRootNodeId As String = "rewardRootNodeId"
        Const saveErrorMsgs As String = "saveErrorMsgs"
        Const siteUrl As String = "siteUrl"
        Const SMTPhost As String = "SMTPhost"
        Const SMTPpassword As String = "SMTPpassword"
        Const SMTPport As String = "SMTPport"
        Const SMTPusername As String = "SMTPusername"
        Const SortBy As String = "Sort By"
        Const StripeApiKey As String = "StripeApiKey"
        Const StripeConnectId As String = "StripeConnectId"
        Const stripeAdditionalFee As String = "stripeAdditionalFee"
        Const stripeCardFeePercent As String = "stripeCardFeePercent"
        Const StripePublicApiKey As String = "StripePublicApiKey"
        Const StripeUrl As String = "https://connect.stripe.com/express/oauth/authorize"
        Const Subcategory As String = "Subcategory"
        Const Subcategories As String = "Subcategories"
        Const succeeded As String = "succeeded"
        Const searchBox As String = "Search Campaigns"
        Const timelineRootNodeId As String = "timelineRootNodeId"
        Const youtubeUrl As String = "https://www.youtube.com/embed/{0}?rel=0"
    End Structure
    Public Structure tabNames
        Const images As String = "images"
        Const content As String = "content"
        Const categories As String = "categories"
        Const phases As String = "phases"
        Const profile As String = "profile"
        Const timeline As String = "timeline"
        Const rewards As String = "rewards"
        Const teamMembers As String = "teamMembers"
    End Structure
    Public Structure Sessions
        Const editMode As String = "editMode"
        Const errorMsg As String = "errorMsg"
        Const FeaturedImageUpdated As String = "FeaturedImageUpdated"
        Const isLoggedIn As String = "isLoggedIn"
        Const isLoggedOut As String = "isLoggedOut"
        Const loginupdated As String = "loginupdated"
        Const newTeamMemberAdded As String = "newTeamMemberAdded"
        Const previousPage As String = "previousPage"
        Const socialMediaSavedSuccessfully As String = "socialMediaSavedSuccessfully"
        Const summaryUpdatedMsg As String = "summaryUpdatedMsg"
        Const successMsg As String = "successMsg"
        Const thankyou As String = "thankyou"
        Const warningMsg As String = "warningMsg"
    End Structure
    Public Structure memberRole
        Const CampaignAdministrator As String = "Campaign Administrator"
        Const CampaignMember As String = "Campaign Member"
        Const member As String = "Member"
        Const TeamAdministrator As String = "Team Administrator"
    End Structure
    Public Structure Crops
        Const campaignFeaturedImage As String = "Campaign Featured Image"
        Const campaignSummaryImage As String = "Campaign Summary Image"
        Const rewardImage As String = "Reward Image"
        Const members As String = "Members"
        Const bannerHomePage As String = "Banner- Home Page"
        Const bannerStandardPages As String = "Banner- Standard Pages"
    End Structure
    'Public Structure customCrops
    '    Const crop56x68 As String = "?width=56&height=68&mode=pad&anchor=bottom&cacheBuster=false&quality=60&compression=60&bgcolor=white"
    '    Const compression60 As String = "?quality=60&compression=60&cacheBuster=false"
    '    Const crop500xAuto As String = "?width=500&mode=pad&anchor=bottom&cacheBuster=false&quality=60&compression=60&bgcolor=white"
    'End Structure
    Public Structure queryParameters
        Const acctData As String = "acctData"
        Const alertMsg As String = "alertMsg"
        Const Category As String = "Category"
        Const code As String = "code"
        Const createAcct As String = "createAcct"
        Const countactUsSubmitted As String = "countactUsSubmitted"
        Const currentUserId As String = "currentUserId"
        Const birthMonth As String = "dobm"
        Const birthDay As String = "dobd"
        Const birthYear As String = "doby"
        Const dateTime As String = "dateTime"
        Const editMode As String = "editMode"
        Const email As String = "email"
        Const entryId As String = "entryId"
        Const error_ As String = "error"
        Const firstName As String = "fname"
        Const fromTeamPage As String = "fromTeamPage"
        Const ListAll As String = "ListAll"
        Const ViewAll As String = "ViewAll"
        Const lastName As String = "lname"
        Const mediaId As String = "mediaId"
        Const newMember As String = "newMember"
        Const nodeId As String = "nodeId"
        Const params As String = "params"
        Const params_campaignInvitation As String = "params_campaignInvitation"
        Const params_teamInvitation As String = "params_teamInvitation"
        Const password As String = "pw"
        Const preAcctId As String = "preAcctId"
        Const Search As String = "Search"
        Const state As String = "state"
        Const stepByStep As String = "stepByStep"
        Const Subcategory As String = "Subcategory"
        Const submissionTime As String = "submissionTime"
        Const tab As String = "tab"
        Const userId As String = "uid"
        Const updatedSuccessfully As String = "updatedSuccessfully"
        Const view As String = "view"
        Const wizardStep As String = "wizardStep"
        Const COMPLETED As String = "COMPLETED"
        Const NAME As String = "NAME"
        Const _DATE As String = "DATE"
        Const TEAMNAME As String = "TEAMNAME"
        Const FAILED As String = "FAILED"
        Const SortBy As String = "SortBy"

    End Structure
    Public Structure mediaType_Strings
        Const Facebook As String = "Facebook"
        Const Twitter As String = "Twitter"
        Const GooglePlus As String = "GooglePlus"
        Const SupportEmail As String = "SupportEmail"
        Const LinkedIn As String = "LinkedIn"
    End Structure
    Public Structure dictionaryEntry
        Const almostThere As String = "Almost There"
        Const campaignComplete As String = "Campaign Complete"
        Const completedSuccessfully As String = "Completed Successfully"
        Const createStripeAcct As String = "Create Stripe Acct"
        Const discoveryPhaseNotes As String = "Discovery Phase Notes"
        Const greatStart As String = "Great Start"
        Const halfWayThere As String = "Half-Way There"
        Const phaseComplete As String = "Phase Complete"
        Const pendingPhase As String = "Pending Phase"
        Const unpublishedNotes As String = "Unpublished Notes"
        Const youDidIt As String = "You Did It"
    End Structure
    Public Structure statusTypeValues
        Const Development As String = "Development"
        Const Unpublished As String = "Unpublished"
        Const Published As String = "Published"
        Const DiscoveryPhase As String = "Discovery Phase"
        Const Phase1Pending As String = "Pending Phase 1"
        Const Phase2Pending As String = "Pending Phase 2"
        Const Phase3Pending As String = "Pending Phase 3"
        Const Phase1Active As String = "Phase 1"
        Const Phase2Active As String = "Phase 2"
        Const Phase3Active As String = "Phase 3"

        Const Phase1Failed As String = "Phase 1 Failed"
        Const Phase2Failed As String = "Phase 2 Failed"
        Const Phase3Failed As String = "Phase 3 Failed"

        Const Phase1Succeeded As String = "Phase 1 Successful"
        Const Phase2Succeeded As String = "Phase 2 Successful"
        Const Phase3Succeeded As String = "Phase 3 Successful"

        Const Complete As String = "Complete"

        Const Active As String = "Active"
        Const Inactive As String = "Inactive"
        Const Pending As String = "Pending"

        'Const Active As String = "Active"
        'Const Inactive As String = "Inactive"
        'Const Incomplete As String = "Incomplete"
    End Structure
    Public Structure pledgeStatusStruct
        Const fulfilled As String = "fulfilled"
        Const canceled As String = "canceled"
        Const declined As String = "declined"
        Const reimbursed As String = "reimbursed"
    End Structure
    Public Structure colorSchemePercentage
        Const bgcolor_10percent As String = "bgcolor_10percent"
        Const bgcolor_20percent As String = "bgcolor_20percent"
        Const bgcolor_30percent As String = "bgcolor_30percent"
        Const bgcolor_40percent As String = "bgcolor_40percent"
        Const bgcolor_50percent As String = "bgcolor_50percent"
        Const bgcolor_60percent As String = "bgcolor_60percent"
        Const bgcolor_70percent As String = "bgcolor_70percent"
        Const bgcolor_80percent As String = "bgcolor_80percent"
        Const bgcolor_90percent As String = "bgcolor_90percent"
        Const bgcolor_100percent As String = "bgcolor_100percent"
    End Structure
    Public Structure searchIndex
        Const ExternalSearcher As String = "ExternalSearcher"
        Const InternalMemberSearcher As String = "InternalMemberSearcher"
        Const InternalSearcher As String = "InternalSearcher"
        Const CampaignSearcher As String = "CampaignSearcher"
        Const CompiledPgSearcher As String = "CompiledPgSearcher"
        Const NavigationSearcher As String = "NavigationSearcher"
    End Structure
    Public Structure searchField
        Const briefSummary As String = "briefSummary"
        Const campaignComplete As String = "campaignComplete"
        Const completionDate As String = "completionDate"
        Const datePublished As String = "datePublished"
        Const description As String = "description"
        Const level As String = "level"
        Const nodeId As String = "nodeId"
        Const nodeName As String = "nodeName"
        Const nodeTypeAlias As String = "nodeTypeAlias"
        Const parentID As String = "parentID"
        Const path As String = "path"
        Const published As String = "published"
        Const showInFooter As String = "showInFooter"
        Const showInNavigation As String = "showInNavigation"
        Const sortOrder As String = "sortOrder"
        Const urlName As String = "urlName"
    End Structure
#End Region

#Region "Methods"
    Public Shared Function UppercaseFirstLetter(ByVal val As String) As String
        ' Test for nothing or empty.
        If String.IsNullOrEmpty(val) Then
            Return val
        End If

        ' Convert to character array.
        Dim array() As Char = val.ToCharArray

        ' Uppercase first character.
        array(0) = Char.ToUpper(array(0))

        ' Return new string.
        Return New String(array)
    End Function
    Public Shared Function FromUnixTime(unixTime As Long) As DateTime
        Dim epoch = New DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        Return epoch.AddSeconds(unixTime)
    End Function
    Public Shared Function getNumeric(ByVal value As String) As String
        'Extracts numbers from string value and returns numeric values only.
        Dim output As StringBuilder = New StringBuilder
        For i = 0 To value.Length - 1
            If (IsNumeric(value(i))) Or (value(i) = ".") Or (value(i) = "-") Then
                output.Append(value(i))
            End If
        Next

        If String.IsNullOrEmpty(output.ToString) OrElse output.ToString = "-" Then
            Return 0
        Else
            If value.Contains("-") Then
                Return (CInt(output.ToString) * -1).ToString
            Else
                Return output.ToString
            End If
        End If
    End Function

    'Public Shared Function Encrypt(ByVal clearText As String) As String
    '    Try
    '        Dim EncryptionKey As String = "MAKV2SPBNI99212"
    '        If Not String.IsNullOrWhiteSpace(clearText) Then
    '            Dim clearBytes As Byte() = Encoding.Unicode.GetBytes(clearText)
    '            Using encryptor As Aes = Aes.Create()
    '                Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D,
    '                 &H65, &H64, &H76, &H65, &H64, &H65,
    '                 &H76})
    '                encryptor.Key = pdb.GetBytes(32)
    '                encryptor.IV = pdb.GetBytes(16)
    '                Using ms As New MemoryStream()
    '                    Using cs As New CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write)
    '                        cs.Write(clearBytes, 0, clearBytes.Length)
    '                        cs.Close()
    '                    End Using
    '                    clearText = Convert.ToBase64String(ms.ToArray())
    '                End Using
    '            End Using
    '        End If

    '        'Return clearText
    '        Return WebUtility.UrlEncode(HttpContext.Current.Server.UrlEncode(clearText))
    '    Catch ex As Exception
    '        Dim sb As New StringBuilder()
    '        sb.AppendLine("\App_Code\Common.vb : Encrypt()")
    '        sb.AppendLine("cipherText:" & clearText)
    '        saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
    '        Return String.Empty
    '    End Try
    'End Function
    'Public Shared Function Decrypt(ByVal cipherText As String) As String
    '    Try
    '        If Not String.IsNullOrWhiteSpace(cipherText) Then
    '            Dim EncryptionKey As String = "MAKV2SPBNI99212"
    '            cipherText = WebUtility.UrlDecode(HttpContext.Current.Server.UrlDecode(cipherText))
    '            cipherText = cipherText.Replace(" ", "+")
    '            Dim cipherBytes As Byte() = Convert.FromBase64String(cipherText)
    '            Using encryptor As Aes = Aes.Create()
    '                Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D,
    '                 &H65, &H64, &H76, &H65, &H64, &H65,
    '                 &H76})
    '                encryptor.Key = pdb.GetBytes(32)
    '                encryptor.IV = pdb.GetBytes(16)
    '                Using ms As New MemoryStream()
    '                    Using cs As New CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write)
    '                        cs.Write(cipherBytes, 0, cipherBytes.Length)
    '                        cs.Close()
    '                    End Using
    '                    cipherText = Encoding.Unicode.GetString(ms.ToArray())
    '                End Using
    '            End Using
    '        End If

    '        Return cipherText
    '    Catch ex As Exception
    '        Dim sb As New StringBuilder()
    '        sb.AppendLine("\App_Code\Common.vb : Decrypt()")
    '        sb.AppendLine("cipherText:" & cipherText)
    '        saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
    '        Return String.Empty
    '    End Try
    'End Function

    Public Shared Function addUpdateUrlQuerystring(ByVal currentUrl As String, ByVal queryName As String, ByVal queryValue As String) As String
        'http://127.0.0.1/edit-campaign/?nodeId=YWuLK5l812KsA3WwBbbElA==&tab=phases&entryId=1272
        'http://127.0.0.1/edit-campaign/?nodeId=YWuLK5l812KsA3WwBbbElA==&tab=categories&entryId=Charity
        'http://127.0.0.1/edit-campaign/?nodeId=YWuLK5l812KsA3WwBbbElA%3d%3d&tab=timeline&entryId=1408

        Try
            'Instantiate variables
            Dim qsDictionary As Dictionary(Of String, String) = New Dictionary(Of String, String)
            Dim splitUrl As String() = currentUrl.Split("?")
            Dim querystr As String()
            Dim newUrl As StringBuilder = New StringBuilder


            'Split querystring if exists and add to dictionary
            If splitUrl.Count > 1 Then
                querystr = splitUrl(1).Split("&")
                For Each qsSet As String In querystr
                    Try
                        Dim qs As String() = qsSet.Split("=")
                        qsDictionary.Add(qs(0), qs(1))
                    Catch ex As Exception
                        Dim sb As New StringBuilder()
                        sb.AppendLine("\App_Code\Common.vb : addUpdateUrlQuerystring()")
                        sb.AppendLine("currentUrl:" & currentUrl)
                        sb.AppendLine("queryName:" & queryName)
                        sb.AppendLine("queryValue:" & queryValue)
                        saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
                    End Try
                Next
            End If

            'Remove submitted query from dictionary
            If qsDictionary.ContainsKey(queryName) Then
                qsDictionary.Remove(queryName)
            End If

            'Add submitted query to dictionary
            qsDictionary.Add(queryName, queryValue)

            'Rebuild url
            newUrl.Append(splitUrl(0))
            If qsDictionary.Count > 0 Then
                newUrl.Append("?")
                For Each item As KeyValuePair(Of String, String) In qsDictionary
                    newUrl.Append(item.Key & "=" & item.Value & "&")
                Next
            End If

            'Remove the last & from the string
            newUrl.Remove(newUrl.ToString.LastIndexOf("&"), 1)

            Return newUrl.ToString
        Catch ex As Exception
            Dim sb As New StringBuilder()
            sb.AppendLine("\App_Code\Common.vb : addUpdateUrlQuerystring()")
            sb.AppendLine("currentUrl:" & currentUrl)
            sb.AppendLine("queryName:" & queryName)
            sb.AppendLine("queryValue:" & queryValue)
            saveErrorMessage(getLoggedInMember, ex.ToString, sb.ToString())
            Return currentUrl & "#error=" & ex.ToString
        End Try
    End Function
    Public Shared Sub setTabCookie(ByVal _tabName As String)
        Try
            If HttpContext.Current.Request.Cookies.Get(queryParameters.tab) Is Nothing Then
                'Create cookie
                Dim myCookie As HttpCookie = New HttpCookie(queryParameters.tab)
                myCookie(queryParameters.tab) = _tabName
                myCookie.Expires = Now.AddDays(1)
                HttpContext.Current.Response.Cookies.Add(myCookie)
            Else
                'Edit cookie
                Dim myCookie As HttpCookie = HttpContext.Current.Request.Cookies.Get(queryParameters.tab)
                myCookie(queryParameters.tab) = _tabName
                myCookie.Expires = Now.AddDays(1)
                HttpContext.Current.Response.Cookies.Add(myCookie)
            End If
        Catch ex As Exception
            'Save error to umbraco
            Dim sb As New StringBuilder
            sb.AppendLine("Common.vb | setTabCookie()")
            sb.AppendLine("_tabName" & _tabName)

            saveErrorMessage(getLoggedInMember(), ex.ToString, sb.ToString)
        End Try
    End Sub
    Public Shared Function ValidUrl(url As String) As Boolean?
        Dim validatedUri As Uri

        If Uri.TryCreate(url, UriKind.Absolute, validatedUri) Then
            '.NET URI validation.
            'If true: validatedUri contains a valid Uri. Check for the scheme in addition.
            Return (validatedUri.Scheme = Uri.UriSchemeHttp OrElse validatedUri.Scheme = Uri.UriSchemeHttps)
        End If
        Return False
    End Function
    Public Shared Function ValidEmail(email As String) As Boolean?
        Try
            Dim address As MailAddress = New MailAddress(email)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Shared Sub saveErrorMessage(ByVal _userId As String, ByVal _exceptionMsg As String, ByVal _generalMsg As String)
        'Obtain value from web.config
        Dim saveErrorMsgs As Boolean = CBool(ConfigurationManager.AppSettings(Miscellaneous.saveErrorMsgs))

        If saveErrorMsgs Then
            'Submit error message to umbraco
            Dim blErrorMsgs As blErrorMsgs = New blErrorMsgs
            blErrorMsgs.saveErrorMessage(_userId, _exceptionMsg, _generalMsg)

            ''WEB SERVICE TEST
            'Dim ws As ErrorMsgHandler = New ErrorMsgHandler()
            'ws.saveMsg(1234, "exception msg", "general msg")
        End If
    End Sub
    Public Shared Function getLoggedInMember() As String
        Dim _userId As String = String.Empty
        'Return if exists
        Try
            If Not String.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name) Then
                Dim userName = HttpContext.Current.User.Identity.Name
                Dim member As IMember = ApplicationContext.Current.Services.MemberService.GetByEmail(userName)
                If member IsNot Nothing Then
                    _userId = member.Id
                End If
            End If
        Catch ex As Exception
            'Save error to umbraco
            Dim sb As New StringBuilder
            sb.AppendLine("Common.vb | getLoggedInMember()")
            saveErrorMessage(getLoggedInMember(), ex.ToString, sb.ToString)
        End Try
        Return _userId
    End Function

    Public Shared Function getPreValueEntry(ByVal _datatypeId As Integer, ByVal _key As Integer) As String
        'Instantiate variables
        Dim dtList As XPathNodeIterator
        Dim dtItem As XPathNodeIterator
        Dim dict As New Dictionary(Of String, String)

        Try  'Get datatype and move to first data position
            dtList = umbraco.library.GetPreValues(_datatypeId)
            dtList.MoveNext() 'move to first
            dtItem = dtList.Current.SelectChildren("preValue", "")

            'Loop thru datatype
            While dtItem.MoveNext()
                If dtItem.Current.GetAttribute("id", "") = _key Then
                    Return dtItem.Current.Value
                End If
            End While
        Catch ex As Exception
            'Save error to umbraco
            Dim sb As New StringBuilder
            sb.AppendLine("Common.vb | getPreValueEntry()")
            sb.AppendLine("_datatypeId: " & _datatypeId)
            sb.AppendLine("_key: " & _key)
            saveErrorMessage(getLoggedInMember(), ex.ToString, sb.ToString)

        End Try
        Return String.Empty
    End Function

    Public Shared Function getMediaURL(ByVal _mediaId As String, Optional ByVal sCropAlias As String = "") As String
        'Instantiate variables
        Dim MediaURL As String = String.Empty

        Try
            If Not (ApplicationContext.Current Is Nothing) Then
                'Do not process if media Id is empty or 0
                If String.IsNullOrEmpty(_mediaId) OrElse CInt(_mediaId) = 0 Then Return String.Empty

                'Obtain media Id
                Dim mediaId As Integer? = getIdFromGuid_byType(_mediaId, UmbracoObjectTypes.Media)

                If Not IsNothing(mediaId) Then
                    'Obtain Image Url
                    'Dim mediaObject = ApplicationContext.Current.Services.MediaService.GetById(mediaId)
                    '
                    Dim UmbracoHelper = New umbraco.Web.UmbracoHelper(umbraco.Web.UmbracoContext.Current)

                    Dim thisFile = UmbracoHelper.TypedMedia(mediaId)

                    If Not IsNothing(thisFile) Then
                        If sCropAlias <> "" Then
                            'get cropped version of image
                            MediaURL = thisFile.GetCropUrl(sCropAlias) & "&quality=60&cacheBuster=false"

                        Else
                            'get regular image url
                            MediaURL = thisFile.Url & "?quality=60&cacheBuster=false"

                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            'Save error to umbraco
            Dim sb As New StringBuilder
            sb.AppendLine("Common.vb | getMediaURL()")
            sb.AppendLine("_mediaId: " & _mediaId)
            sb.AppendLine("sCropAlias: " & sCropAlias)
            'saveErrorMessage(ex.ToString, sb.ToString)
            saveErrorMessage(getLoggedInMember(), ex.ToString, sb.ToString)
            MediaURL = String.Empty
        End Try

        'Return url
        Return MediaURL
    End Function
    Public Shared Function getMediaName(ByVal _mediaId As String) As String

        'Instantiate variables
        Dim mediaURL As String = String.Empty
        Dim mediaName As String = String.Empty

        Try
            If _mediaId <> "0" Then

                If Not (ApplicationContext.Current Is Nothing) Then


                    'Obtain media Id
                    Dim mediaId As Integer? = getIdFromGuid_byType(_mediaId, UmbracoObjectTypes.Media)

                    If Not IsNothing(mediaId) Then
                        'Obtain Image Url
                        Dim mediaObject = ApplicationContext.Current.Services.MediaService.GetById(mediaId)
                        If IsNothing(mediaObject) Then
                            Return String.Empty
                        Else
                            mediaName = mediaObject.Name
                            'Remove file type from string
                            mediaName = mediaName.Split(".")(0)
                            'remove underscores and hyphens from string
                            mediaName = mediaName.Replace("_", " ").Replace("-", " ")
                        End If

                    End If
                End If
            End If

        Catch ex As Exception
            'Save error to umbraco
            Dim sb As New StringBuilder
        sb.AppendLine("Common.vb | getMediaName()")
            sb.AppendLine("_mediaId: " & _mediaId)
            saveErrorMessage(getLoggedInMember(), ex.ToString, sb.ToString)

        mediaName = String.Empty
        End Try

        'Return url
        Return mediaName
    End Function
    Public Shared Function getNode(ByVal _id As String) As Node
        Try
            'Instantiate variables
            Dim varUdi As Udi

            'Attempt to obtain the node id from the provided guid
            If Udi.TryParse(_id, varUdi) Then
                Dim guid = GuidUdi.Parse(varUdi.ToString())
                Dim entityService As IEntityService = ApplicationContext.Current.Services.EntityService

                Dim response As Attempt(Of Integer) = entityService.GetIdForKey(guid.Guid, UmbracoObjectTypes.Document)
                If response.Success Then
                    Return New Node(response.Result)
                End If
            ElseIf IsNumeric(_id) Then
                'Is already an integer.  return value.
                Return New Node(_id)
            Else
                'If no integer was returned, return nothing
                Return Nothing
            End If

        Catch ex As Exception
            'Save error to umbraco
            Dim sb As New StringBuilder
            sb.AppendLine("Common.vb | getMediaName()")
            sb.AppendLine("_mediaId: " & _id)
            saveErrorMessage(getLoggedInMember(), ex.ToString, sb.ToString)

            Return Nothing
        End Try
    End Function
    Public Shared Function getIdFromGuid_byType(ByVal _id As String, ByVal _objectType As UmbracoObjectTypes) As Integer?
        Try
            'Instantiate variables
            Dim varUdi As Udi

            'Attempt to obtain the node id from the provided id or guid
            If IsNumeric(_id) Then
                'Is already an integer.  return value.
                Return CInt(_id)
            ElseIf Udi.TryParse(_id, varUdi) Then
                Dim guid = GuidUdi.Parse(varUdi.ToString())
                Dim entityService As IEntityService = ApplicationContext.Current.Services.EntityService
                Dim response As Attempt(Of Integer) = entityService.GetIdForKey(guid.Guid, _objectType)

                If response.Success Then
                    Return response.Result
                End If
            Else
                'If no integer was returned, return nothing
                Return Nothing
            End If

        Catch ex As Exception
            'Save error to umbraco
            Dim sb As New StringBuilder
            sb.AppendLine("Common.vb | getIdFromGuid_byType()")
            sb.AppendLine("_id: " & _id)
            sb.AppendLine("_objectType: " & _objectType.ToString)
            saveErrorMessage(getLoggedInMember(), ex.ToString, sb.ToString)

            Return Nothing
        End Try
    End Function

    Public Shared Function getStatusTypeText(ByVal status As statusType) As String
        '
        Select Case status
            Case statusType.Complete
                Return statusTypeValues.Complete
            Case statusType.DiscoveryPhase
                Return statusTypeValues.DiscoveryPhase
            Case statusType.Phase1Active
                Return statusTypeValues.Phase1Active
            Case statusType.Phase1Failed
                Return statusTypeValues.Phase1Failed
            Case statusType.Phase1Pending
                Return statusTypeValues.Phase1Pending
            Case statusType.Phase1Succeeded
                Return statusTypeValues.Phase1Succeeded
            Case statusType.Phase2Active
                Return statusTypeValues.Phase2Active
            Case statusType.Phase2Failed
                Return statusTypeValues.Phase2Failed
            Case statusType.Phase2Pending
                Return statusTypeValues.Phase2Pending
            Case statusType.Phase2Succeeded
                Return statusTypeValues.Phase2Succeeded
            Case statusType.Phase3Active
                Return statusTypeValues.Phase3Active
            Case statusType.Phase3Failed
                Return statusTypeValues.Phase3Failed
            Case statusType.Phase3Pending
                Return statusTypeValues.Phase3Pending
            Case statusType.Phase3Succeeded
                Return statusTypeValues.Phase3Succeeded
            Case statusType.Unpublished
                Return statusTypeValues.Unpublished
            Case Else
                Return String.Empty
        End Select
    End Function
    Public Shared Function getCampaignId(ByVal _id As Integer) As Integer?
        Try
            Dim _uHelper As Uhelper = New Uhelper()
            Dim thisNode As IPublishedContent = _uHelper.Get_IPublishedContentByID(_id)
            If thisNode.DocumentTypeAlias = docTypes.Campaign Then
                Return _id
            Else
                Return getCampaignId(thisNode.Parent.Id)
            End If
        Catch ex As Exception
            Dim asb As New StringBuilder()
            asb.AppendLine("\App_Code\Common.vb : getCampaignId()")
            asb.AppendLine("_id:" & _id)
            saveErrorMessage(getLoggedInMember, ex.ToString, asb.ToString())

        End Try
    End Function
    Public Shared Function getManagementEmails_byCampaignId(ByVal _campaignId As Integer) As List(Of String)
        'Scope variables
        Dim lstOfEmails As New List(Of String)

        Try
            'Instantiate variables
            Dim blMembers As New blMembers
            Dim cs As ContentService = ApplicationContext.Current.Services.ContentService
            Dim campaignNode As IContent = cs.GetById(_campaignId)
            Dim teamNode As IContent = campaignNode.Parent
            Dim umbracoHelper = New UmbracoHelper(UmbracoContext.Current)
            Dim pcCampaign As IPublishedContent = umbracoHelper.TypedContent(_campaignId)
            Dim pcCampaignMembers As IPublishedContent = pcCampaign.Children.Where(Function(x As IPublishedContent) (x.DocumentTypeAlias.Equals(docTypes.campaignMembers))).FirstOrDefault

            'Obtain list of all team administrators and add their emails to the list
            If teamNode.HasProperty(nodeProperties.teamAdministrators) Then
                For Each admin As String In teamNode.GetValue(nodeProperties.teamAdministrators).ToString.Split(",").ToList()
                    lstOfEmails.Add(blMembers.getMemberEmail_byId(admin))
                    Dim altEmail As String = blMembers.getUsersAltEmail_byId(admin)
                    If Not String.IsNullOrEmpty(altEmail) Then lstOfEmails.Add(altEmail)
                Next
            End If

            'Obtain list of all campaign administrators and add their emails to the list
            If Not IsNothing(pcCampaignMembers) AndAlso Not IsNothing(pcCampaignMembers.Children) Then
                For Each campaignMember In pcCampaignMembers.Children
                    If campaignMember.HasProperty(nodeProperties.campaignManager) AndAlso campaignMember.GetPropertyValue(Of Boolean)(nodeProperties.campaignManager) = True Then
                        If Not lstOfEmails.Contains(campaignMember.GetPropertyValue(Of String)(nodeProperties.campaignMember)) Then
                            lstOfEmails.Add(blMembers.getMemberEmail_byId(campaignMember.GetPropertyValue(Of String)(nodeProperties.campaignMember)))
                            Dim altEmail As String = blMembers.getUsersAltEmail_byId(campaignMember.GetPropertyValue(Of String)(nodeProperties.campaignMember))
                            If Not String.IsNullOrEmpty(altEmail) Then lstOfEmails.Add(altEmail)
                        End If
                    End If
                Next
            End If

        Catch ex As Exception
            'Save error to umbraco
            Dim sb As New StringBuilder
            sb.AppendLine("Common.vb | getManagementEmails_byCampaignId()")
            sb.AppendLine("_campaignId: " & _campaignId)
            saveErrorMessage(getLoggedInMember(), ex.ToString, sb.ToString)
        End Try

        Return lstOfEmails
    End Function

    Public Shared Function addToNotes(ByVal nodeId As Integer, ByVal msg As String) As String
        'Instantiate variables
        Dim umbracoHelper = New UmbracoHelper(UmbracoContext.Current)
        Dim ipNode As IPublishedContent = umbracoHelper.TypedContent(nodeId)
        Dim sb As New StringBuilder

        Try   'Add new data to notes
            sb.AppendLine(msg)
            sb.AppendLine("----------------------------")

            If Not IsNothing(ipNode) Then
                'Extract existing data from notes
                If ipNode.HasProperty(nodeProperties.notes) AndAlso ipNode.HasValue(nodeProperties.notes) Then
                    sb.AppendLine(ipNode.GetPropertyValue(Of String)(nodeProperties.notes))
                End If
            Else
                'Instantiate variables
                Dim cs As IContentService = ApplicationContext.Current.Services.ContentService
                Dim icNode As IContent = cs.GetById(nodeId)

                'Extract existing data from notes
                If icNode.HasProperty(nodeProperties.notes) Then
                    sb.AppendLine(icNode.GetValue(Of String)(nodeProperties.notes))
                End If
            End If
        Catch ex As Exception
            Dim asb As New StringBuilder()
            asb.AppendLine("\App_Code\Common.vb : addToNotes()")
            asb.AppendLine("nodeId:" & nodeId)
            asb.AppendLine("msg:" & msg)
            saveErrorMessage(getLoggedInMember, ex.ToString, asb.ToString())

        End Try
        Return sb.ToString
    End Function

#End Region

#Region "Handles"
    Public Sub New()
    End Sub
#End Region
End Class