# Certificates assigment

## Background

__Warranty Expert__ is insurance agent company that allows shops to provide insurance and warranty services for their end customers.

Once service is purchased - customers receive insurance certificate that holds information about the insured item, customer and how long the certificate is valid.

## Task
Fork this repo and implement:
* backend - insurance certificate creation that satisfies business and technical requirements;
* frontend - an Angular component to display recommended pricing plans;
* full stack - both backend and frontend.

Use Visual Studio or Visual Studio Code to launch this project. It will launch both .NET backend and Angular apps.

### Requirements for backend
* Customer must be at least 18 years old when buying insurance certificate.
* Certificate is considered valid from the moment it is created and lasts 1 year. The last day is valid for the whole day. For example if a customer buys certificate on __2022-09-01 14:30__, then certificate is valid from __2022-09-01 14:30__ until 
__2023-09-01 00:00__.
* Certificate number consists of 5 digits. Each number must be unique and incremented by 1. For example first certificates will have these numbers: 00001, 00002, 00003.
* Certificate sum depends on insured item price. For example, if insured item price is 75, then certificate sum is 15. If insured item price does not fit any range, then certificate must not be allowed to be created.

| Insured item price range | Certificate sum |
| --- | ----------- |
| 20.00 - 50.00 | 8 |
| 50.01 - 100.00 | 15 |
| 100.01 - 200.00 | 25 |

### Technical requirements
* Error message must be provided in case of validation error
* Certificate must be saved and provided in the list after successful validation

### Requirements for frontend
Create an empty structure for the future application. Create menu with items:
* Certificates
* Claims
* Pricing plans
* Reports
  * Sales reports
  * Claims reports

For each menu item create an empty page, but reuse current certificate list page and certificate entry page.

In the Pricing plans page you are asked to implement a small Angular component that processes and displays recommended pricing plans from a hierarchical dataset. The dataset is tree-structured, with categories (non-leaf nodes) containing nested plans. Only leaf nodes have a price. The goal is to traverse the tree, pick recommended plans within a price range, sort them and display them as cards in a simple template.

The pricing plans are available from backend endpoint GET pricingPlans. The new component should filter and sort the data:
* pick only recommended plans with price between 100 and 200 (both inclusive);
* sort plans by price (descending).

Render each plan in a card-style element. Display name and price. Include all names from the branch, separated by " / ": "All plans / Standard / Family". Minimal styling is fine; focus on correct data display.

The solution should be written in a way that is testable, but writing actual tests is optional.

## Evaluation criteria
* Business and technical requirement fulfilment
* Code readability (variables, method names, class names)
* Code formatting
* Code structure
* Business requirment unit testability
* Bonus for unit tests
