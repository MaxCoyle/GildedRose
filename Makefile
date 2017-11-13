INPUT_FILE ?= input.txt
.PHONY: build test run

build:
	cd src/gilded-rose && dotnet build

test:
	cd test/gilded-rose-test && \
	dotnet test

run:
	cd src/gilded-rose && \
	dotnet run $(INPUT_FILE)
