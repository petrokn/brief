package common

import "regexp"

func GetGroupsData(regEx *regexp.Regexp, matchedString string) (paramsMap map[string]string) {
	match := regEx.FindStringSubmatch(matchedString)

	paramsMap = make(map[string]string)
	for i, name := range regEx.SubexpNames() {
		if i > 0 && i <= len(match) {
			paramsMap[name] = match[i]
		}
	}
	return
}

func Check(e error) {
	if e != nil {
		panic(e)
	}
}

func Contains(slice []string, str string) bool {
	for _, a := range slice {
		if a == str {
			return true
		}
	}
	return false
}
